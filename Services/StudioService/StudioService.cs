using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Studio;
using MovieApi.Services.UserService;

namespace MovieApi.Services.StudioService
{
    public class StudioService : IStudioService
    {
        private readonly AppDbContext _context;

        private readonly IUserService _userService;

        public StudioService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<(Studio, IEnumerable<Seat>)> FindByIdAsync(string id)
        {
            var studio = await _context
                .Studios
                .OrderBy(s => s.Code)
                .FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false) ?? 
                throw new DllNotFoundException("Studio not found");
            
            var seats = await _context
                .Seats
                .Where(s => s.StudioId == id && s.Deleted == false)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Column)
                .ToListAsync() ?? throw new DllNotFoundException("Seats not found");

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
                CreatedBy = _userService.GetUserId(),
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
            studio.UpdatedBy = _userService.GetUserId();
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
                throw new BadHttpRequestException("Id is required");
            
            var (studio, seats) = await FindByIdAsync(id);

            studio.Deleted = true;
            studio.UpdatedBy = _userService.GetUserId();
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
                    Column = startColumn
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