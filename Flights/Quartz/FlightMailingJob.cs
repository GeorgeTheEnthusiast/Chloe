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

namespace Flights.Quartz
{
    [DisallowConcurrentExecution]
    public class FlightMailingJob : IJob
    {
        private readonly IFlightMailingService _flightMailingService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public FlightMailingJob(IFlightMailingService flightMailingService)
        {
            if (flightMailingService == null) throw new ArgumentNullException("flightMailingService");

            _flightMailingService = flightMailingService;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Sending flights through e-mails...");

                _flightMailingService.SendResults(DateTime.Now);

                _logger.Info("Sending flights through e-mails completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
