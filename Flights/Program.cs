using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.NBPCurrency;
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
                Console.WriteLine("Uruchomiono usługę Flights.");

                Bootstrapper.Register();

                if (args != null && args.Any() && args[0] != null && args[0] == "/console")
                {
                    FlightsNtService flightsNtService = new FlightsNtService();
                    flightsNtService.StartQuartzJob();
                }
                else
                {
                    var flightsService = new FlightsNtService();

                    ServiceBase.Run(new ServiceBase[]
                    {
                        flightsService,
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
