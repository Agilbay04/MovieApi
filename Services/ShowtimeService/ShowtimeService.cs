using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Exceptions;
using MovieApi.Requests.Showtime;
using MovieApi.Responses.Seat;
using MovieApi.Services.UserService;
using MovieApi.Utilities;

namespace MovieApi.Services.ShowtimeService
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly AppDbContext _context;

        private readonly DateUtil _dateUtil;

        private readonly IUserService _userService;

        public ShowtimeService(AppDbContext context, DateUtil dateUtil, IUserService userService)
        {
            _context = context;
            _dateUtil = dateUtil;
            _userService = userService;
        }

        public async Task<(Showtime, List<SeatResponse>)> FindByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BadRequestException("Id is required");

            var showtime = await _context
                .Showtimes
                .FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false) ?? 
                throw new BadRequestException("Showtime not found");

            var listSeatUnavailable = await _context
                .Bookings
                .Join(_context.BookingSeats, b => b.Id, bs => bs.BookingId, (b, bs) => new { b, bs })
                .Where(x => x.b.ShowtimeId == showtime.Id && x.bs.Deleted == false)
                .Select(x => new SeatResponse 
                {
                    Id = x.bs.SeatId,
                    SeatNumber = x.bs.Seat.SeatNumber,
                    Row = x.bs.Seat.Row,
                    Column = x.bs.Seat.Column,
                    IsAvailable = false
                })
                .ToListAsync();
            
            var listSeatAvailable = await _context
                .Seats
                .Where(x => x.StudioId == showtime.StudioId && 
                    !listSeatUnavailable.Select(x => x.Id).Contains(x.Id) && 
                    x.Deleted == false)
                .Select(x => new SeatResponse
                {
                    Id = x.Id,
                    SeatNumber = x.SeatNumber,
                    Row = x.Row,
                    Column = x.Column,
                    IsAvailable = true
                })
                .ToListAsync();
            
            var listSeat = listSeatAvailable
                .Concat(listSeatUnavailable)
                .OrderBy(x => x.Row)
                .ThenBy(x => x.Column)
                .ToList();
                        
            return (showtime, listSeat);
        }

        public async Task<IEnumerable<Showtime>> FindAllAsync()
        {
            return await _context
                .Showtimes
                .Where(s => s.Deleted == false)
                .OrderBy(s => s.PlayDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Showtime> CreateAsync(CreateShowtimeRequest req)
        {
            var startTime = _dateUtil.GetStringToTime(req.StartTime);
            var playDate = _dateUtil.GetStringToDate(req.PlayDate);

            if (playDate <= DateTime.Now)
                throw new BadRequestException("Play date cannot be in the past");

            var timeAndDateIsUsed = await _context
                .Showtimes
                .Where(s => s.StudioId == req.StudioId && s.StartTime == startTime && 
                    s.PlayDate == playDate && s.Deleted == false)
                .FirstOrDefaultAsync();

            if (timeAndDateIsUsed != null)
                throw new BadRequestException("The schedule is already used by movie " + timeAndDateIsUsed.Movie?.Title);
            
            var priceId = await SetPriceId(playDate, req.MovieId);

            var showtime = new Showtime
            {
                MovieId = req.MovieId,
                StudioId = req.StudioId,
                PriceId = priceId,
                StartTime = startTime,
                PlayDate = playDate,
                CreatedBy = _userService.GetUserId(),
            };
            await _context.Showtimes.AddAsync(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        public async Task<Showtime> UpdateAsync(UpdateShowtimeRequest req, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Id is required");

            var startTime = _dateUtil.GetStringToTime(req.StartTime);
            var playDate = _dateUtil.GetStringToDate(req.PlayDate);

            if (playDate <= DateTime.Now)
                throw new BadRequestException("Play date cannot be in the past");
            
            if (startTime <= DateTime.Now.TimeOfDay)
                throw new BadRequestException("Start time cannot be in the past");
            
            var timeAndDateIsUsed = await _context
                .Showtimes
                .Where(s => s.Id != id && s.StartTime == startTime && 
                    s.StudioId == req.StudioId && s.PlayDate == playDate && 
                    s.Deleted == false)
                .FirstOrDefaultAsync();

            if (timeAndDateIsUsed != null)
                throw new BadRequestException("The schedule is already used by movie " + timeAndDateIsUsed.Movie?.Title);
            
            var priceId = await SetPriceId(playDate, req.MovieId);

            var (showtime, _) = await FindByIdAsync(id);
            showtime.MovieId = req.MovieId;
            showtime.StudioId = req.StudioId;
            showtime.PriceId = priceId;
            showtime.StartTime = startTime;
            showtime.PlayDate = playDate;
            showtime.UpdatedBy = _userService.GetUserId();
            _context.Showtimes.Update(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        public async Task<Showtime> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BadRequestException("Id is required");

            var (showtime, _) = await FindByIdAsync(id);
            showtime.Deleted = true;
            showtime.UpdatedBy = _userService.GetUserId();
            _context.Showtimes.Update(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        private async Task<string> SetPriceId(DateTime dateTime, string movieId)
        {
            var movie = await _context
                .Movies
                .Where(m => m.Id == movieId && m.Deleted == false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Movie not found");

            var price = await _context
                .Prices
                .Where(p => p.Deleted == false)
                .ToArrayAsync() ?? throw new NotFoundException("Price not found");

            if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return price.Where(p => p.Code == AppConstant.PRICE_WEEKEND)
                            .FirstOrDefault()?.Id ?? 
                            throw new NotFoundException("Price not found");
            }
            else if (dateTime.DayOfWeek == movie.ReleaseDate.DayOfWeek)
            {
                return price.Where(p => p.Code == AppConstant.PRICE_PRIMETIME)
                            .FirstOrDefault()?.Id ?? 
                            throw new NotFoundException("Price not found");
            }
            else
            {
                return price.Where(p => p.Code == AppConstant.PRICE_WEEKDAY)
                            .FirstOrDefault()?.Id ?? 
                            throw new NotFoundException("Price not found");
            }
        }
    }
}