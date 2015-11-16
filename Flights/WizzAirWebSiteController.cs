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
using OpenQA.Selenium.Support.UI;

namespace Flights
{
    public class WizzAirWebSiteController : IWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly ICarrierQuery _carrierQuery;
        private readonly IWizzAirCalendarConverter _wizzAirCalendarConverter;
        private Carrier _carrier;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;

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
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_carrier.Website);
        }

        private void FillCityFrom(SearchCriteria searchCriteria)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.ClassName("city-from"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(searchCriteria.CityFrom.Name);
            fromCityWebElement.SendKeys(Keys.Enter);
        }

        private void FillCityTo(SearchCriteria searchCriteria)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.ClassName("city-to"));
            
            toCityWebElement.Click();
            toCityWebElement.SendKeys(searchCriteria.CityTo.Name);
            toCityWebElement.SendKeys(Keys.Enter);
        }

        private void FillDate(SearchCriteria searchCriteria)
        {
            IWebElement datePickerWebElement = _driver.FindElement(By.CssSelector("div[id='ui-datepicker-div']"));

            ClickWebElement(datePickerWebElement);

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
            string printedYear = _driver.FindElement(By.CssSelector("span[class='ui-datepicker-year']")).Text;
            int year = int.Parse(printedYear);

            while (year != searchCriteria.DepartureDate.Year)
            {
                _driver.FindElement(By.ClassName("ui-datepicker-next")).Click();

                printedYear = _driver.FindElement(By.CssSelector("span[class='ui-datepicker-year']")).Text;
                year = int.Parse(printedYear);
            }
        }

        private void SetCalendarMonth(SearchCriteria searchCriteria)
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

        private void SetCalendarDay(SearchCriteria searchCriteria)
        {
            var calendarTableWebElement = _driver.FindElement(By.ClassName("ui-datepicker-calendar"));
            var daysInTableWebElement = calendarTableWebElement.FindElements(By.TagName("td"));
            var daysInTableWIthCorrectValue = daysInTableWebElement.Where(x => x.FindElement(By.TagName("a")).Text == searchCriteria.DepartureDate.Day.ToString());
            var correctDayWebElement = daysInTableWIthCorrectValue.First(x => x.GetAttribute("class") != " ui-datepicker-other-month ");

            correctDayWebElement.Click();
        }

        private void FindFlights()
        {
            IWebElement searchButton = _driver.FindElement(By.CssSelector("button[class='buttonN button primary search preloader ']"));
            ClickWebElement(searchButton);
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            if (_carrier == null)
                _carrier = _carrierQuery.GetCarrierByType(CarrierType.WizzAir);

            List<Flight> result = new List<Flight>();

            if (searchCriteria.Carrier.Id != _carrier.Id)
                return result;

            NavigateToUrl();

            FillCityFrom(searchCriteria);

            FillCityTo(searchCriteria);

            FillDate(searchCriteria);

            FindFlights();
            
            var flightSlides = _webDriverWait.Until(x => x.FindElements(By.CssSelector("div[class='flight-row']")));

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
                SearchCriteria = searchCriteria,
                IsDirect = true
            };
            
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
                
            AddCurrency(ref result, priceValue);
            
            return result;
        }

        private void AddCurrency(ref Flight flightToAddCurrency, string price)
        {
            price = price.Trim('\r', '\n', ' ');
            string[] priceArray = price.Split(new[] { "&nbsp;", " " }, StringSplitOptions.RemoveEmptyEntries);

            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = priceArray.Last()
            });
            flightToAddCurrency.Price = int.Parse(string.Join("", priceArray.Reverse().Skip(1).Reverse()), NumberStyles.Currency);
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
