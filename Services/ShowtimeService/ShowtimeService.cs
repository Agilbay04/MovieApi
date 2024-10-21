using System.Drawing;
using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Showtime;
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

        public async Task<Showtime> FindByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Id is required");

            var showtime = await _context
                .Showtimes
                .FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false) ?? 
                throw new Exception("Showtime not found");
            
            return showtime;
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
                throw new Exception("Play date cannot be in the past");

            var timeAndDateIsUsed = await _context
                .Showtimes
                .Where(s => s.StudioId == req.StudioId && s.StartTime == startTime && 
                    s.PlayDate == playDate && s.Deleted == false)
                .FirstOrDefaultAsync();

            if (timeAndDateIsUsed != null)
                throw new Exception("The schedule is already used by movie " + timeAndDateIsUsed.Movie?.Title);
            
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
                throw new Exception("Play date cannot be in the past");
            
            if (startTime <= DateTime.Now.TimeOfDay)
                throw new Exception("Start time cannot be in the past");
            
            var timeAndDateIsUsed = await _context
                .Showtimes
                .Where(s => s.Id != id && s.StartTime == startTime && 
                    s.StudioId == req.StudioId && s.PlayDate == playDate && 
                    s.Deleted == false)
                .FirstOrDefaultAsync();

            if (timeAndDateIsUsed != null)
                throw new Exception("The schedule is already used by movie " + timeAndDateIsUsed.Movie?.Title);
            
            var priceId = await SetPriceId(playDate, req.MovieId);

            var showtime = await FindByIdAsync(id);
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
                throw new Exception("Id is required");

            var showtime = await FindByIdAsync(id);
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
                .FirstOrDefaultAsync() ?? throw new Exception("Movie not found");

            var price = await _context
                .Prices
                .Where(p => p.Deleted == false)
                .ToArrayAsync() ?? throw new Exception("Price not found");

            if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return price.Where(p => p.Code == AppConstant.PRICE_WEEKEND)
                            .FirstOrDefault()?.Id ?? 
                            throw new Exception("Price not found");
            }
            else if (dateTime.DayOfWeek == movie.ReleaseDate.DayOfWeek)
            {
                return price.Where(p => p.Code == AppConstant.PRICE_PRIMETIME)
                            .FirstOrDefault()?.Id ?? 
                            throw new Exception("Price not found");
            }
            else
            {
                return price.Where(p => p.Code == AppConstant.PRICE_WEEKDAY)
                            .FirstOrDefault()?.Id ?? 
                            throw new Exception("Price not found");
            }
        }
    }
}