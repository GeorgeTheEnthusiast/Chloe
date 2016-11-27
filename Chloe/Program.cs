using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chloe.Converters;
using Chloe.Domain.Command;
using Chloe.Domain.Query;
using Chloe.Dto;
using Chloe.NBPCurrency;
using NLog;
using NLog.Common;
using OpenQA.Selenium.Interactions;

namespace Chloe
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Uruchomiono usługę Chloe.");

                Bootstrapper.Register();

                if (args != null && args.Any() && args[0] != null && args[0] == "/console")
                {
                    ChloeNtService ChloeNtService = new ChloeNtService();
                    ChloeNtService.StartQuartzJob();
                }
                else
                {
                    var ChloeService = new ChloeNtService();

                    ServiceBase.Run(new ServiceBase[]
                    {
                        ChloeService,
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
