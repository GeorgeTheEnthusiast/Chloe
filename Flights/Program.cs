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
using OpenQA.Selenium.Interactions;

namespace Flights
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Bootstrapper.Register();

//                FlightSearchController flightSearchController = new FlightSearchController(
//                    Bootstrapper.Container.Resolve<IFlightService>(),
//                    Bootstrapper.Container.Resolve<IFlightsCommand>(),
//                    Bootstrapper.Container.Resolve<ISearchCriteriaQuery>(),
//                    Bootstrapper.Container.Resolve<IWebDriver>());

                //while(flightSearchController.StartSearch() == false);

                IFlightMailingService flightMailingService = new FlightMailingService(Bootstrapper.Container.Resolve<IFlightsQuery>());

                flightMailingService.SendResults(DateTime.Now);

                Console.WriteLine("Szukanie przelotów zakończone.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
