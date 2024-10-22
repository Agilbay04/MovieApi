using log4net;
using MovieApi.Database;
using Quartz;

namespace MovieApi.Jobs
{
    public class DeleteShowtimeJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(DeleteShowtimeJob));

        private readonly AppDbContext _context;

        public DeleteShowtimeJob(AppDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _log.Info("Start delete showtime job");
            var showtimes = _context.Showtimes
                .Where(s => s.PlayDate == DateTime.Today && s.Deleted == false)
                .ToList();
            var today = DateTime.Today;
            
            foreach (var showtime in showtimes)
            {
                var startTime = showtime.StartTime;
                var duration = showtime.Movie?.Duration;
                var fullDuration = startTime.Add(TimeSpan.FromMinutes(duration ?? 0));
                var timeNow = DateTime.Now.TimeOfDay;

                if (fullDuration <= timeNow)
                {
                    showtime.Deleted = true;
                    _context.Showtimes.Update(showtime);

                    _log.Info($"Delete showtime {showtime.Id}");
                }
            }

            _log.Info("Finish delete showtime job");
            await _context.SaveChangesAsync();
        }    
    }
}