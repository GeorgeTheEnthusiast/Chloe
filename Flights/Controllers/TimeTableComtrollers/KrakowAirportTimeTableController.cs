using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Flights.Dto.Enums.FlightWebsite;

namespace Flights.Controllers.TimeTableComtrollers
{
    public class KrakowAirportTimeTableController : ITimeTableController
    {
        private readonly ITimeTableCommand _timeTableCommand;
        private readonly IWebDriver _driver;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;

        public KrakowAirportTimeTableController(IWebDriver driver,
            ITimeTableCommand timeTableCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (timeTableCommand == null) throw new ArgumentNullException("timeTableCommand");

            _driver = driver;
            _timeTableCommand = timeTableCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(6));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(12));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }
        
        public void CreateTimeTable()
        {
            GoToTimeTableSite();

            ExpandAllTimeTables();

            SelectDepartures();

            Create();
        }

        private void GoToTimeTableSite()
        {
            var passengerInfos = _driver.FindElement(By.PartialLinkText("informacje-o-lotach"));
            passengerInfos.Click();

            var timeTableScheduler = _driver.FindElement(By.PartialLinkText("rozklad-rejsow-regularnych"));
            timeTableScheduler.Click();
        }

        private void ExpandAllTimeTables()
        {
            var select = _driver.FindElement(By.Name("airpid"));
            SelectElement selectElement = new SelectElement(select);
            selectElement.SelectByIndex(-1);
        }

        private void SelectDepartures()
        {
            var tab = _driver.FindElement(By.ClassName("tab2"));

            tab.Click();
        }

        private void Create()
        {
            var table = _driver.FindElement(By.CssSelector("table[class='default-table']"));


        }
    }
}
