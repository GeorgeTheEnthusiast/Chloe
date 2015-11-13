using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Quartz;
using NLog;
using OpenQA.Selenium;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Flights
{
    partial class FlightsNtService : ServiceBase
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public FlightsNtService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger.Debug("Starting searching thread...");

            StartQuartzJob();

            _logger.Debug("Search thread successfully started.");
        }

        protected override void OnStop()
        {
            _logger.Debug("Searching stopped...");
        }

        public void StartQuartzJob()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
                
            IScheduler sched = schedFact.GetScheduler();
            sched.JobFactory = Bootstrapper.Container.Resolve<IJobFactory>();
            sched.Start();
                
            IJobDetail job = JobBuilder.Create<SearchFlightsJob>()
                .WithIdentity("searchJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("daily_3am_and_5pm_Trigger")
                .StartNow()
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(17, 00))
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(03, 00))
                .Build();

            sched.ScheduleJob(job, trigger);
        }
    }
}
