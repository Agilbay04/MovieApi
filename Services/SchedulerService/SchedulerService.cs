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

            //JOB 1: CANCEL BOOKING JOB
            var cancelBookingJob = JobBuilder.Create<CancelBookingJob>()
                .WithIdentity("CancelBookingJob", "group1")
                .Build();

            var cancelBookingTrigger = TriggerBuilder.Create()
                .WithIdentity("CancelBookingTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(cancelBookingJob, cancelBookingTrigger);

            //JOB 2: DELETE SHOWTIME JOB
            var deleteShowtimeJob = JobBuilder.Create<DeleteShowtimeJob>()
                .WithIdentity("DeleteShowtimeJob", "group2")
                .Build();

            var deleteShowtimeTrigger = TriggerBuilder.Create()
                .WithIdentity("DeleteShowtimeTrigger", "group2")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(deleteShowtimeJob, deleteShowtimeTrigger);

            await scheduler.Start();
        }
    }
}