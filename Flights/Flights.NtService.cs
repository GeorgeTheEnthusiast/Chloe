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
            //                
            //            IJobDetail searchJob = JobBuilder.Create<SearchFlightsJob>()
            //                .WithIdentity("searchJob")
            //                .Build();
            //
            //            ITrigger searchTrigger = TriggerBuilder.Create()
            //                .WithIdentity("daily_3pm_Trigger")
            //                .StartNow()
            //                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(15, 00))
            //                .Build();
            //
            //            sched.ScheduleJob(searchJob, searchTrigger);
            //
            //            IJobDetail netjob = JobBuilder.Create<FlightsNetJob>()
            //                .WithIdentity("searchJob")
            //                .Build();
            //
            //            ITrigger netTrigger = TriggerBuilder.Create()
            //                .WithIdentity("one_a_week_5pm_Trigger")
            //                .StartNow()
            //                .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 17, 00))
            //                .Build();
            //
            //            //sched.ScheduleJob(netjob, netTrigger);
            //
            //            IJobDetail nbpCurrencyjob = JobBuilder.Create<NBPCurrencyDownloaderJob>()
            //                .WithIdentity("nbpCurrencyjob")
            //                .Build();
            //
            //            ITrigger nbpCurrencyTrigger = TriggerBuilder.Create()
            //                .WithIdentity("one_a_week_4pm_Trigger")
            //                .StartNow()
            //                .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 16, 00))
            //                .Build();
            //
            //            sched.ScheduleJob(nbpCurrencyjob, nbpCurrencyTrigger);

                        IJobDetail timeTableJob = JobBuilder.Create<CreateTimeTableJob>()
                            .WithIdentity("timeTableJob")
                            .Build();
            
                        ITrigger timeTableTrigger = TriggerBuilder.Create()
                            .WithIdentity("monthly_3pm_Trigger")
                            .StartNow()
                            //.WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(01, 00, 00))
                            .Build();
            
                        sched.ScheduleJob(timeTableJob, timeTableTrigger);
        }
    }
}
