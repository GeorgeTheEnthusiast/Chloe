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
    public class SearchFlightsJob : IJob
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            try
            { 
                _logger.Info("Registering dependencies...");
                
                Bootstrapper.Register();

                FlightSearchController flightSearchController = new FlightSearchController(
                    Bootstrapper.Container.Resolve<IFlightService>(),
                    Bootstrapper.Container.Resolve<IFlightsCommand>(),
                    Bootstrapper.Container.Resolve<ISearchCriteriaQuery>(),
                    Bootstrapper.Container.Resolve<IWebDriver>());

                while (flightSearchController.StartSearch() == false) ;

                _logger.Info("Searching for the cheapest prices completed.");

                IFlightMailingService flightMailingService = new FlightMailingService(
                    Bootstrapper.Container.Resolve<IFlightsQuery>(),
                    Bootstrapper.Container.Resolve<INotificationsReceiverQuery>(),
                    Bootstrapper.Container.Resolve<ICountryQuery>());

                flightMailingService.SendResults(DateTime.Now);

                _logger.Info("Sending flights through e-mails completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
