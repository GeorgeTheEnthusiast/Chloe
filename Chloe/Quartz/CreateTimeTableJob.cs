using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chloe.Controllers.TimeTableControllers;
using Chloe.Domain.Command;
using Chloe.Domain.Query;
using NLog;
using OpenQA.Selenium;
using Quartz;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Spi;

namespace Chloe.Quartz
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
                _logger.Info("Error searching the Chloe!");
                _logger.Error(ex);
            }
        }
    }
}
