using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chloe.Domain.Command;
using Chloe.Domain.Query;
using Chloe.NBPCurrency;
using NLog;
using OpenQA.Selenium;
using Quartz;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Spi;

namespace Chloe.Quartz
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
