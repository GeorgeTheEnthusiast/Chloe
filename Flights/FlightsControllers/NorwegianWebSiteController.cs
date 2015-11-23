using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Flights.Dto.Enums.FlightWebsite;

namespace Flights.FlightsControllers
{
    public class NorwegianWebSiteController : IWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly INorwegianDateConverter _norwegianDateConverter;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;

        public NorwegianWebSiteController(IWebDriver driver,
            ICurrienciesCommand currienciesCommand,
            INorwegianDateConverter norwegianDateConverter,
            IFlightWebsiteQuery flightWebsiteQuery
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (norwegianDateConverter == null) throw new ArgumentNullException("norwegianDateConverter");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");

            _driver = driver;
            _currienciesCommand = currienciesCommand;
            _norwegianDateConverter = norwegianDateConverter;
            _flightWebsiteQuery = flightWebsiteQuery;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }

        private void FillCityFrom(SearchCriteria searchCriteria)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("input[placeholder='Twój punkt wyjścia']"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(searchCriteria.CityFrom.Name);
            fromCityWebElement.SendKeys(Keys.Enter);
        }

        private void FillCityTo(SearchCriteria searchCriteria)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("input[placeholder='Dokąd chcesz się wybrać?']"));

            //toCityWebElement.Click();
            toCityWebElement.SendKeys(searchCriteria.CityTo.Name);
            toCityWebElement.SendKeys(Keys.Enter);
        }

        private void FillDate(SearchCriteria searchCriteria)
        {
            IWebElement datePickerSectionWebElement = _driver.FindElement(By.CssSelector("section[id='outboundDate']"));
            IWebElement datePickerWebElement =
                datePickerSectionWebElement.FindElement(By.CssSelector("input[type='text'][class='calendar__input']"));

            datePickerWebElement.Click();
            
            SetCalendar(searchCriteria);
        }

        private void SetCalendar(SearchCriteria searchCriteria)
        {
            SetCalendarYear(searchCriteria);

            SetCalendarMonth(searchCriteria);

            SetCalendarDay(searchCriteria);
        }

        private void SetCalendarYear(SearchCriteria searchCriteria)
        {
            IWebElement calendarHeaderWebElement =
                _driver.FindElement(
                    By.CssSelector("table[role='grid']"))
                    .FindElement(By.TagName("thead"))
                    .FindElement(By.CssSelector("button[role='heading']"));
            string printedDate = calendarHeaderWebElement.FindElement(By.TagName("strong")).Text;

            DateTime printedDateTime = _norwegianDateConverter.Convert(printedDate);

            while (printedDateTime.Year != searchCriteria.DepartureDate.Year)
            {
                _driver.FindElement(By.CssSelector("button[type='button'][class='btn btn-default btn-sm pull-right']")).Click();

                calendarHeaderWebElement =
                _driver.FindElement(
                    By.CssSelector("table[role='grid']"))
                    .FindElement(By.TagName("thead"))
                    .FindElement(By.CssSelector("button[role='heading']"));
                printedDate = calendarHeaderWebElement.FindElement(By.TagName("strong")).Text;

                printedDateTime = _norwegianDateConverter.Convert(printedDate);
            }
        }

        private void SetCalendarMonth(SearchCriteria searchCriteria)
        {
            IWebElement calendarHeaderWebElement =
                _driver.FindElement(
                    By.CssSelector("table[role='grid']"))
                    .FindElement(By.TagName("thead"))
                    .FindElement(By.CssSelector("button[role='heading']"));
            string printedDate = calendarHeaderWebElement.FindElement(By.TagName("strong")).Text;

            DateTime printedDateTime = _norwegianDateConverter.Convert(printedDate);

            while (printedDateTime.Month != searchCriteria.DepartureDate.Month)
            {
                _driver.FindElement(By.CssSelector("button[type='button'][class='btn btn-default btn-sm pull-right']")).Click();

                calendarHeaderWebElement =
                _driver.FindElement(
                    By.CssSelector("table[role='grid']"))
                    .FindElement(By.TagName("thead"))
                    .FindElement(By.CssSelector("button[role='heading']"));
                printedDate = calendarHeaderWebElement.FindElement(By.TagName("strong")).Text;

                printedDateTime = _norwegianDateConverter.Convert(printedDate);
            }
        }

        private void SetCalendarDay(SearchCriteria searchCriteria)
        {
            var calendarTableWebElement = _driver.FindElement(
                By.CssSelector("table[role='grid']"))
                .FindElement(By.TagName("tbody"));
            
            var daysInTableWebElement = calendarTableWebElement.FindElements(By.TagName("button"));
            var daysInTableWIthIncorrectValue = daysInTableWebElement.Where(x => x.FindElement(By.TagName("span")).GetAttribute("class") == "text-muted");
            var daysInTableWithCorrectValue = daysInTableWebElement.Except(daysInTableWIthIncorrectValue);
            
            var correctDayWebElement = daysInTableWithCorrectValue.First(x => x.Text == searchCriteria.DepartureDate.Day.ToString("00"));

            correctDayWebElement.Click();
        }

        private void FindFlights()
        {
            IWebElement searchButton = _driver.FindElement(By.Id("searchButton"));
            ClickWebElement(searchButton);
        }

        private void MakeTicketOneWay()
        {
            IWebElement oneWayTicketWebElement = _driver.FindElement(By.Id("tripType"))
                .FindElements(By.TagName("label"))[1];
            oneWayTicketWebElement.Click();
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.Norwegian);

            List<Flight> result = new List<Flight>();

            return result;

            if (searchCriteria.FlightWebsite.Id != _flightWebsite.Id)
                return result;

            NavigateToUrl();

            FillCityFrom(searchCriteria);

            FillCityTo(searchCriteria);

            MakeTicketOneWay();

            FillDate(searchCriteria);

            GoToTheCheapestPricesCalendar();

            FindFlights();

            var flightDaysAll =
                _webDriverWait.Until(x => x.FindElement(By.CssSelector("table[class='fareCalendarTable']")))
                .FindElements(By.TagName("td"));
            var flightDaysWeek =
                _webDriverWait.Until(x => x.FindElement(By.CssSelector("table[class='fareCalendarTable']")))
                .FindElements(By.CssSelector("td[class='week']"));
            var flightDays = flightDaysAll.Except(flightDaysWeek);

            foreach (var day in flightDays)
            {
                var flight = GetOneItemFromCalendar(day, searchCriteria);

                if (flight != null)
                    result.Add(flight);
            }

            return result;
        }

        private void GoToTheCheapestPricesCalendar()
        {
            IWebElement oneWayTicketWebElement = _driver.FindElement(By.CssSelector("div[data-model='model.request.resultType']"))
                .FindElements(By.TagName("label"))[1];
            oneWayTicketWebElement.Click();
        }

        private Flight GetOneItemFromCalendar(IWebElement webElement, SearchCriteria searchCriteria)
        {
            Flight result = new Flight()
            {
                SearchDate = DateTime.Now,
                SearchCriteria = searchCriteria
            };
            
            var flightWebElement = webElement.FindElement(By.TagName("div"));
            string flightType = flightWebElement.GetAttribute("class");
                
            if (flightType == "fareCalDayDirect")
                result.IsDirect = true;
            else if (flightType == "fareCalDayDirectLowest")
                result.IsDirect = true;
            else if (flightType == "fareCalDayTransit")
                result.IsDirect = false;
            else if (flightType == "fareCalDayTransitLowest")
                result.IsDirect = false;
            else if (flightType == "fareCalNoDay")
                return null;
            else if (flightType == "fareCalNoFlight")
                return null;

            string onClickValue = flightWebElement.GetAttribute("onclick");

            result.Price = GetPriceFromCarousel(onClickValue);
            result.DepartureTime = GetDateFromCarousel(onClickValue);

            if (DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(-2)) < 0 ||
                DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(2)) > 0)
                return null;

            AddCurrency(ref result);
          
            return result;
        }

        private DateTime GetDateFromCarousel(string text)
        {
            string dateString = text.Substring(text.IndexOf("'") + 1, 8);
            return DateTime.ParseExact(dateString, "ddyyyyMM", CultureInfo.InvariantCulture);
        }

        private int GetPriceFromCarousel(string text)
        {
            //FareCal_OnClick(this, '07201512', '948', 'Outbound');
            int indexFrom = FindIndexOfNthOccurence(text, "'", 3);
            int indexTo = FindIndexOfNthOccurence(text, "'", 4);
            string price = text.Substring(indexFrom + 1, indexTo - indexFrom - 1);
            return int.Parse(price);
        }

        private int FindIndexOfNthOccurence(string text, string textToSearch, int nthOccurence)
        {
            int result = -1;

            for (int i = 0; i < nthOccurence; i++)
            {
                result = text.IndexOf(textToSearch, result + 1);
            }

            return result;
        }

        private void AddCurrency(ref Flight flightToAddCurrency)
        {
            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = "zł"
            });
        }

        private void ClickWebElement(IWebElement webElement)
        {
            IWait<IWebElement> wait = new DefaultWait<IWebElement>(webElement);
            wait.Timeout = TimeSpan.FromSeconds(5);
            wait.Until(x => x.Displayed);
            webElement.Click();
        }
    }
}
