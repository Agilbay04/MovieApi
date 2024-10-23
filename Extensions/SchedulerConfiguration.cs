using MovieApi.Services.SchedulerService;

namespace MovieApi.Extensions
{
    public static class SchedulerConfiguration
    {
        public static async Task ConfigureSchedulerAsync(this WebApplication app)
        {
            var schedulerService = app.Services.GetRequiredService<ISchedulerService>();
            await schedulerService.ScheduleJobsAsync();
        }        
    }
}