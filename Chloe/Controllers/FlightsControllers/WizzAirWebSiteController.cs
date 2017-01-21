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

namespace Flights.Controllers.FlightsControllers
{
    public class WizzAirWebSiteController : IWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly IWizzAirCalendarConverter _wizzAirCalendarConverter;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierCommand _carrierCommand;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;
        private readonly string ThisCityIsNotAvailable = "This city is not available";

        public WizzAirWebSiteController(IWebDriver driver,
            ICurrienciesCommand currienciesCommand,
            IWizzAirCalendarConverter wizzAirCalendarConverter,
            IFlightWebsiteQuery flightWebsiteQuery,
            ICarrierCommand carrierCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (wizzAirCalendarConverter == null) throw new ArgumentNullException("wizzAirCalendarConverter");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (carrierCommand == null) throw new ArgumentNullException("carrierCommand");

            _driver = driver;
            _currienciesCommand = currienciesCommand;
            _wizzAirCalendarConverter = wizzAirCalendarConverter;
            _flightWebsiteQuery = flightWebsiteQuery;
            _carrierCommand = carrierCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(6));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(20));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.Id("search-departure-station"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Enter);
        }

        private void FillCityTo(string cityName)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.Id("search-arrival-station"));
            
            toCityWebElement.Click();
            toCityWebElement.SendKeys(cityName);
            toCityWebElement.SendKeys(Keys.Enter);
        }

        private bool IsCityToIsAvailable(string cityFrom, string cityTo)
        {
            try
            {
                var autocompleteBox = _driver
                    .FindElements(By.CssSelector("div[class='box-autocomplete inHeader']"));

                if (autocompleteBox.Count == 0)
                {
                    return true;
                }

                string cityToDropDownListText = autocompleteBox[1]
                    .FindElement(By.ClassName("wrap"))
                    .Text;

                if (cityToDropDownListText == ThisCityIsNotAvailable)
                {
                    _logger.Warn("Connection [{0}] --> [{1}] is not available", cityFrom, cityTo);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }

            return true;
        }

        private void FillDate(SearchCriteria searchCriteria)
        {
            IWebElement datePickerWebElement = _driver.FindElement(By.Id("search-departure-date"));

            //ClickWebElement(datePickerWebElement);

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
            string printedYear = _driver
                .FindElement(By.ClassName("calendar"))
                .FindElement(By.ClassName("pika-title"))
                .FindElements(By.TagName("div"))[1].Text;
            int year = int.Parse(printedYear);

            while (year != searchCriteria.DepartureDate.Year)
            {
                _driver
                .FindElement(By.CssSelector("button[class='pika-next']"))
                .Click();

                printedYear = _driver
                .FindElement(By.ClassName("calendar"))
                .FindElement(By.ClassName("pika-title"))
                .FindElements(By.TagName("div"))[1].Text;
                year = int.Parse(printedYear);
            }
        }

        private void SetCalendarMonth(SearchCriteria searchCriteria)
        {
            string printedMonth = _driver
                .FindElement(By.ClassName("calendar"))
                .FindElement(By.ClassName("pika-title"))
                .FindElements(By.TagName("div"))[0].Text;
            int month = _wizzAirCalendarConverter.ConvertMonth(printedMonth);

            while (month != searchCriteria.DepartureDate.Month)
            {
                _driver
                .FindElement(By.CssSelector("button[class='pika-next']"))
                .Click();

                printedMonth = _driver
                .FindElement(By.ClassName("calendar"))
                .FindElement(By.ClassName("pika-title"))
                .FindElements(By.TagName("div"))[0].Text;
                month = _wizzAirCalendarConverter.ConvertMonth(printedMonth);
            }
        }

        private void SetCalendarDay(SearchCriteria searchCriteria)
        {
            _driver
                .FindElement(By.ClassName("calendar"))
                .FindElement(
                    By.CssSelector(
                        $"button[class='pika-button pika-day'][data-pika-day='{searchCriteria.DepartureDate.Day}']"))
                .Click();
        }

        private void FindFlights()
        {
            IWebElement searchButton = _driver.FindElement(By.CssSelector("button[data-test='flight-search-submit']"));
            ClickWebElement(searchButton);
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.WizzAir);

            List<Flight> result = new List<Flight>();

            //return result;

            if (searchCriteria.FlightWebsite.Id != _flightWebsite.Id)
                return result;

            NavigateToUrl();

            FillCityFrom(searchCriteria.CityFrom.Name);

            FillCityTo(searchCriteria.CityTo.Name);

            if (IsCityToIsAvailable(searchCriteria.CityFrom.Name, searchCriteria.CityTo.Name) == false)
                return result;

            FillDate(searchCriteria);

            FindFlights();
            
            var flightSlides = _webDriverWait.Until(
                x => x.FindElement(By.CssSelector("ul[class='booking-flow__flight-select__chart__days']"))
                .FindElements(By.TagName("li")));

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
            
            var dateWebElement = webElement.FindElement(By.TagName("time"));

            string dateLong = dateWebElement.GetAttribute("datetime");

            result.DepartureTime = DateTime.Parse(dateLong);
                
            var priceSlide = webElement.FindElement(By.TagName("span"));
            string priceValue = priceSlide.Text;
                
            AddCurrency(ref result, priceValue);
            result.Carrier = _carrierCommand.Merge("WizzAir");

            return result;
        }

        private void AddCurrency(ref Flight flightToAddCurrency, string price)
        {
            price = price.Trim('\r', '\n', ' ');
            string[] priceArray = price.Split(new[] { "&nbsp;", " " }, StringSplitOptions.RemoveEmptyEntries);
            string valueToParse = string.Join("", priceArray.Reverse().Skip(1).Reverse())
                .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            string currency = priceArray.Last().Replace("+", "");

            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = currency
            });
            flightToAddCurrency.Price = decimal.Parse(valueToParse, NumberStyles.Currency, CultureInfo.InvariantCulture);
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
