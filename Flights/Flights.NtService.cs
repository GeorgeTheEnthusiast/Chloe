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
            QuartzJobFactory.ScheduleJob<SearchFlightsJob>(SimpleScheduleBuilder.Create());
            //QuartzJobFactory.ScheduleJob<SearchFlightsJob>(CronScheduleBuilder.DailyAtHourAndMinute(15, 00));
            //QuartzJobFactory.ScheduleJob<FlightsNetJob>(SimpleScheduleBuilder.Create());
            //QuartzJobFactory.ScheduleJob<FlightsNetJob>(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 15, 00));
            //QuartzJobFactory.ScheduleJob<NBPCurrencyDownloaderJob>(SimpleScheduleBuilder.Create());
            //QuartzJobFactory.ScheduleJob<NBPCurrencyDownloaderJob>(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Tuesday, 15, 00));
            //QuartzJobFactory.ScheduleJob<CreateTimeTableJob>(SimpleScheduleBuilder.Create());
            //QuartzJobFactory.ScheduleJob<CreateTimeTableJob>(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(01, 15, 00));
        }
    }
}
