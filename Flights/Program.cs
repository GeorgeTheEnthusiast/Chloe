using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using NLog;
using NLog.Common;
using OpenQA.Selenium.Interactions;

namespace Flights
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
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

                while(flightSearchController.StartSearch() == false);

                _logger.Info("Szukanie przelotów zakończone.");

                IFlightMailingService flightMailingService = new FlightMailingService(
                    Bootstrapper.Container.Resolve<IFlightsQuery>(), 
                    Bootstrapper.Container.Resolve<INotificationsReceiverQuery>(),
                    Bootstrapper.Container.Resolve<ICountryQuery>());

                flightMailingService.SendResults(DateTime.Now);

                _logger.Info("Wysyłanie najtańszych lotów zakończone.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            Console.ReadKey();
        }
    }
}
