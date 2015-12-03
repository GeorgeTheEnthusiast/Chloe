using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Controllers.TimeTableComtrollers;
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
    public class CreateTimeTableJob : IJob
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Starting to create TimeTables...");

                var implementations = Bootstrapper.Container.ResolveAll<ITimeTableController>();

                foreach (var implementation in implementations)
                {
                    try
                    {
                        _logger.Info("Starting [{0}]", implementation.GetType());

                        implementation.CreateTimeTable();

                        _logger.Info("Starting [{0}] - completed!", implementation.GetType());
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Error creating TimeTable for [{0}]", implementation.GetType());
                        _logger.Error(ex);
                    }
                }

                _logger.Info("Starting to create TimeTables completed...");
            }
            catch (Exception ex)
            {
                _logger.Info("Error searching the flights!");
                _logger.Error(ex);
            }
        }
    }
}
