using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Flights.Quartz
{
    public static class QuartzJobFactory
    {
        public static void ScheduleJob<T>(IScheduleBuilder scheduleBuilder) where T : IJob
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            var jobName = Guid.NewGuid().ToString();
            var triggerName = Guid.NewGuid().ToString();

            IScheduler sched = schedFact.GetScheduler();
            sched.JobFactory = Bootstrapper.Container.Resolve<IJobFactory>();
            sched.Start();

            IJobDetail searchJob = JobBuilder.Create<T>()
                .WithIdentity(jobName)
                .Build();

            ITrigger searchTrigger = TriggerBuilder.Create()
                .WithIdentity(triggerName)
                .StartNow()
                .WithSchedule(scheduleBuilder)
                .Build();

            sched.ScheduleJob(searchJob, searchTrigger);
        }
    }
}
