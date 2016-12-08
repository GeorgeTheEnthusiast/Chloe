using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Controllers.FlightsControllers;
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
    public class FlightsNetJob : IJob
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Creating a flights net...");

                var netImplementations = Bootstrapper.Container.ResolveAll<IFlightsNetController>();

                foreach (var implementation in netImplementations)
                {
                    implementation.CreateNet();
                }

                _logger.Info("Creating a flights net completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
