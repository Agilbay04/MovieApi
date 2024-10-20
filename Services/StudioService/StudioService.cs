using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Studio;

namespace MovieApi.Services.StudioService
{
    public class StudioService : IStudioService
    {
        private readonly AppDbContext _context;

        public StudioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(Studio, IEnumerable<Seat>)> FindByIdAsync(string id)
        {
            var studio = await _context
                .Studios
                .FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false) ?? 
                throw new Exception("Studio not found");
            
            var seats = await _context
                .Seats
                .Where(s => s.StudioId == id && s.Deleted == false)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Column)
                .ToListAsync() ?? throw new Exception("Seats not found");

            return (studio, seats);
        }

        public async Task<IEnumerable<Studio>> FindAllAsync()
        {
            return await _context
                .Studios
                .Where(s => s.Deleted == false)
                .ToListAsync();
        }

        public async Task<(Studio, IEnumerable<Seat>)> CreateAsync(CreateStudioRequest req)
        {
            var studio = new Studio
            {
                Code = await CreateCodeStudio(),
                Name = req.Name,
                Facility = req.Facility,
                TotalSeats = req.TotalSeats,
            };
            await _context.Studios.AddAsync(studio);
            await _context.SaveChangesAsync();

            var seats = CreateSeats(studio.Id, studio.TotalSeats, req.RowPerSeats)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Column)
                .ToList();
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
            return (studio, seats);
        }

        public async Task<(Studio, IEnumerable<Seat>)> UpdateAsync(UpdateStudioRequest req, string id)
        {
            var (studio, seats) = await FindByIdAsync(id);
            
            studio.Name = req.Name;
            studio.Facility = req.Facility;
            studio.TotalSeats = req.TotalSeats;
            _context.Studios.Update(studio);
            
            if (req.TotalSeats != studio.TotalSeats || req.RowPerSeats != 0)
            {
                var seatsToDelete = _context
                    .Seats.Where(s => s.StudioId == id && s.Deleted == false)
                    .ToList();
                _context.Seats.RemoveRange(seatsToDelete);

                var newSeats = CreateSeats(studio.Id, req.TotalSeats, req.RowPerSeats)
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .ToList();
                await _context.Seats.AddRangeAsync(newSeats);
            }

            await _context.SaveChangesAsync();
            return (studio, seats);
        }

        public async Task<Studio> DeleteAsync(string id)
        {
            if (id == null)
                throw new Exception("Id is required");
            
            var studioInUsed = await _context
                .Seats
                .Where(s => s.StudioId == id 
                    && s.Deleted == false
                    && s.IsAvailable == false)
                .FirstOrDefaultAsync();
            
            if (studioInUsed != null)
                throw new Exception("There is a seat in used in this studio");

            var (studio, seats) = await FindByIdAsync(id);

            studio.Deleted = true;
            _context.Studios.Update(studio);
            _context.Seats.RemoveRange(seats);
            await _context.SaveChangesAsync();
            return studio;
        }

        private async Task<string> CreateCodeStudio()
        {
            var studio = await FindAllAsync();
            if (!studio.Any())
                return "ST1";
            
            return $"ST{studio.Count() + 1}";
        }


        private static List<Seat> CreateSeats(string studioId, int totalSeats, int rowPerSeats)
        {
            var seats = new List<Seat>();
            char startRow = 'A';
            int startColumn = 1;

            for (int i = 0; i < totalSeats; i++)
            {
                seats.Add(new Seat
                {
                    StudioId = studioId,
                    SeatNumber = $"{startRow}{startColumn}",
                    Row = startRow.ToString(),
                    Column = startColumn,
                    IsAvailable = true
                });

                startColumn++;

                if (startColumn > rowPerSeats)
                {
                    startColumn = 1;
                    startRow++;
                }
            }
            return seats;
        }
    }
}