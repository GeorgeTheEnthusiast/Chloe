using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Dto.Enums;
using Flights.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Flights
{
    public class WizzAirWebSiteController : IWizzAirWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly ICarrierQuery _carrierQuery;
        private readonly IWizzAirCalendarConverter _wizzAirCalendarConverter;
        private Carrier _carrier;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public WizzAirWebSiteController(IWebDriver driver,
            ICurrienciesCommand currienciesCommand,
            ICarrierQuery carrierQuery,
            IWizzAirCalendarConverter wizzAirCalendarConverter)
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (carrierQuery == null) throw new ArgumentNullException("carrierQuery");
            if (wizzAirCalendarConverter == null) throw new ArgumentNullException("wizzAirCalendarConverter");

            _driver = driver;
            _currienciesCommand = currienciesCommand;
            _carrierQuery = carrierQuery;
            _wizzAirCalendarConverter = wizzAirCalendarConverter;
        }

        public void NavigateToUrl()
        {
            _carrier = _carrierQuery.GetCarrierByType(CarrierType.WizzAir);

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_carrier.Website);

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        public void FillCityFrom(SearchCriteria searchCriteria)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement fromCityWebElement = _driver.FindElement(By.ClassName("city-from"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(searchCriteria.CityFrom.Name);
            fromCityWebElement.SendKeys(Keys.Enter);
        }

        public void FillCityTo(SearchCriteria searchCriteria)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement toCityWebElement = _driver.FindElement(By.ClassName("city-to"));
            
            toCityWebElement.Click();
            toCityWebElement.SendKeys(searchCriteria.CityTo.Name);
            toCityWebElement.SendKeys(Keys.Enter);
        }

        public void FillDate(SearchCriteria searchCriteria)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement datePickerWebElement = _driver.FindElement(By.CssSelector("div[id='ui-datepicker-div']"));

            datePickerWebElement.Click();

            SetCalendar(searchCriteria);
        }

        public void SetCalendar(SearchCriteria searchCriteria)
        {
            SetCalendarYear(searchCriteria);

            SetCalendarMonth(searchCriteria);

            SetCalendarDay(searchCriteria);
        }

        void SetCalendarYear(SearchCriteria searchCriteria)
        {
            string printedYear = _driver.FindElement(By.CssSelector("span[class='ui-datepicker-year']")).Text;
            int year = int.Parse(printedYear);

            while (year != searchCriteria.DepartureDate.Year)
            {
                _driver.FindElement(By.ClassName("ui-datepicker-next")).Click();

                printedYear = _driver.FindElement(By.CssSelector("span[class='ui-datepicker-year']")).Text;
                year = int.Parse(printedYear);
            }
        }

        void SetCalendarMonth(SearchCriteria searchCriteria)
        {
            string printedMonth = _driver.FindElement(By.CssSelector("span[class='ui-datepicker-month']")).Text;
            int month = _wizzAirCalendarConverter.ConvertMonth(printedMonth);

            while (month != searchCriteria.DepartureDate.Month)
            {
                _driver.FindElement(By.ClassName("ui-datepicker-next")).Click();

                printedMonth = _driver.FindElement(By.CssSelector("span[class='ui-datepicker-month']")).Text;
                month = _wizzAirCalendarConverter.ConvertMonth(printedMonth);
            }
        }

        void SetCalendarDay(SearchCriteria searchCriteria)
        {
            var calendarTableWebElement = _driver.FindElement(By.ClassName("ui-datepicker-calendar"));
            var daysInTableWebElement = calendarTableWebElement.FindElements(By.TagName("td"));
            var daysInTableWIthCorrectValue = daysInTableWebElement.Where(x => x.FindElement(By.TagName("a")).Text == searchCriteria.DepartureDate.Day.ToString());
            var correctDayWebElement = daysInTableWIthCorrectValue.First(x => x.GetAttribute("class") != " ui-datepicker-other-month ");

            correctDayWebElement.Click();
        }

        public void FindFlights()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement searchButton = _driver.FindElement(By.CssSelector("button[class='buttonN button primary search preloader ']"));
            searchButton.Click();

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            List<Flight> result = new List<Flight>();

            var flightSlides = _driver.FindElements(By.CssSelector("div[class='flight-row']"));

            foreach (var slide in flightSlides)
            {
                var flight = GetOneItemFromCarousel(slide, searchCriteria);

                if (flight != null)
                    result.Add(flight);
            }

            return result;
        }

        private Flight GetOneItemFromCarousel(IWebElement webElement, SearchCriteria searchCriteria)
        {
            Flight result = new Flight()
            {
                SearchDate = DateTime.Now,
                SearchCriteria = searchCriteria
            };

            try
            {
                var dateWebElement = webElement.FindElement(By.CssSelector("div[class='flight-data flight-date']"));

                if (dateWebElement.GetAttribute("innerHTML") == "Wylot i przylot")
                    return null;

                if (dateWebElement.GetAttribute("class") == "flight-row disabled")
                    return null;

                string dateLong = dateWebElement.FindElement(By.TagName("span"))
                    .GetAttribute("data-flight-departure");

                result.DepartureTime = DateTime.Parse(dateLong);
                
                var priceSlide = webElement.FindElement(By.CssSelector("label[class='flight flight-data flight-fare flight-radio flight-fare-type--basic flight-fare--active']"));
                string priceValue = priceSlide.GetAttribute("innerHTML");
                priceValue = priceValue.Remove(0, priceValue.LastIndexOf(">") + 1);

                result.SearchValidationText = "OK";
                
                AddCurrency(ref result, priceValue);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return null;
            }
            return result;
        }

        public void AddCurrency(ref Flight flightToAddCurrency, string price)
        {
            price = price.Trim('\r', '\n', ' ');
            string[] priceArray = price.Split(new[] { "&nbsp;", " " }, StringSplitOptions.RemoveEmptyEntries);

            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = priceArray.Last()
            });
            flightToAddCurrency.Price = int.Parse(string.Join("", priceArray.Reverse().Skip(1).Reverse()), NumberStyles.Currency);
        }
    }
}
