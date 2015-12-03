using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.NBPCurrency;
using NLog;
using OpenQA.Selenium;
using Quartz;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Spi;

namespace Flights.Quartz
{
    [DisallowConcurrentExecution]
    public class NBPCurrencyDownloaderJob : IJob
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Downloading the latest NBP currencies...");

                var implementation = Bootstrapper.Container.Resolve<IXmlDownloader>();

                implementation.Download();

                _logger.Info("Downloading the latest NBP currencies completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
