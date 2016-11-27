using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chloe.Controllers.FlightsControllers;
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
    public class FlightsNetJob : IJob
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Creating a Chloe net...");

                var netImplementations = Bootstrapper.Container.ResolveAll<IFlightsNetController>();

                foreach (var implementation in netImplementations)
                {
                    implementation.CreateNet();
                }

                _logger.Info("Creating a Chloe net completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
