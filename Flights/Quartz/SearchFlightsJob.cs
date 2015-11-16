using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Domain.Command;
using Flights.Domain.Query;
using NLog;
using OpenQA.Selenium;
using Quartz;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Spi;

namespace Flights.Quartz
{
    [DisallowConcurrentExecution]
    public class SearchFlightsJob : IJob
    {
        private readonly IFlightSearchController _flightSearchController;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public SearchFlightsJob(IFlightSearchController flightSearchController)
        {
            if (flightSearchController == null) throw new ArgumentNullException("flightSearchController");

            _flightSearchController = flightSearchController;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _flightSearchController.StartSearch();

                ISchedulerFactory schedFact = new StdSchedulerFactory();

                IScheduler sched = schedFact.GetScheduler();
                sched.JobFactory = Bootstrapper.Container.Resolve<IJobFactory>();
                sched.Start();

                IJobDetail job = JobBuilder.Create<FlightMailingJob>()
                    .WithIdentity("mailingJob")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("start_now_Trigger")
                    .StartNow()
                    .Build();

                //sched.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
