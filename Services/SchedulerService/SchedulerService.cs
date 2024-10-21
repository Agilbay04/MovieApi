using MovieApi.Jobs;
using Quartz;

namespace MovieApi.Services.SchedulerService
{
    public class SchedulerService : ISchedulerService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public SchedulerService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task ScheduleJobsAsync()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<CancelBookingJob>()
                .WithIdentity("CancelBookingJob", "group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("CancelBookingTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            await scheduler.Start();
        }
    }
}