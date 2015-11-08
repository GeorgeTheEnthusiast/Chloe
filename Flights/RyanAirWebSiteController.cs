using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Dto.Enums;
using Flights.Exceptions;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Flights
{
    public class RyanAirWebSiteController : IRyanAirWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly IRyanAirDateConverter _ryanAirDateConverter;
        private readonly ICarrierQuery _carrierQuery;

        private Carrier _carrier;
        private const string FlightsIsNotAvailable = "Brak lotów w tym dniu";
        
        public RyanAirWebSiteController(IWebDriver driver, 
            ICurrencyConverter currencyConverter, 
            ICurrienciesCommand currienciesCommand, 
            IRyanAirDateConverter ryanAirDateConverter, 
            ICarrierQuery carrierQuery)
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currencyConverter == null) throw new ArgumentNullException("currencyConverter");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (ryanAirDateConverter == null) throw new ArgumentNullException("ryanAirDateConverter");
            if (carrierQuery == null) throw new ArgumentNullException("carrierQuery");

            _driver = driver;
            _currencyConverter = currencyConverter;
            _currienciesCommand = currienciesCommand;
            _ryanAirDateConverter = ryanAirDateConverter;
            _carrierQuery = carrierQuery;
        }

        public void NavigateToUrl()
        {
            if (_carrier == null)
                _carrier = _carrierQuery.GetCarrierByType(CarrierType.RyanAir);

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_carrier.Website);

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        public void MakeTicketOneWay()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IReadOnlyCollection<IWebElement> typeOfTicketWebElements = _driver.FindElements(By.ClassName("flight-search-type-option"));
            IWebElement oneWayTicketWebElement = typeOfTicketWebElements.First(x => x.Text == "W jedną stronę");
            oneWayTicketWebElement.Click();
        }

        public void FillCityFrom(SearchCriteria searchCriteria)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-from']"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(fromCityWebElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.SendKeys(searchCriteria.CityFrom.Name);
            actions.SendKeys(Keys.Tab);
            actions.Build().Perform();

            if (IsInputWasFilledCorrectly(searchCriteria.CityFrom.Name, fromCityWebElement) == false)
            {
                throw new InputWasNotFilledCorrectlyException()
                {
                    Name = "CityFrom"
                };
            }
        }

        public void FillCityTo(SearchCriteria searchCriteria)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-to']"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(toCityWebElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.SendKeys(searchCriteria.CityTo.Name);
            actions.SendKeys(Keys.Enter);
            actions.Build().Perform();

            if (IsInputWasFilledCorrectly(searchCriteria.CityTo.Name, toCityWebElement) == false)
            {
                throw new InputWasNotFilledCorrectlyException()
                {
                    Name = "CityTo"
                };
            }
        }

        public void FillDate(SearchCriteria searchCriteria)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement datePickerWebElement = _driver.FindElement(By.ClassName("date-input"));
            IWebElement dayPickerWebElement = datePickerWebElement.FindElement(By.ClassName("dd"));
            IWebElement monthPickerWebElement = datePickerWebElement.FindElement(By.ClassName("mm"));
            IWebElement yearPickerWebElement = datePickerWebElement.FindElement(By.ClassName("yyyy"));

            SetDatePickerDigital(dayPickerWebElement, searchCriteria.DepartureDate.Day.ToString("00"));
            SetDatePickerDigital(monthPickerWebElement, searchCriteria.DepartureDate.Month.ToString("00"));
            SetDatePickerDigital(yearPickerWebElement, searchCriteria.DepartureDate.Year.ToString("0000"));
        }

        public void SetDatePickerDigital(IWebElement webElement, string value)
        {
            if (webElement == null) throw new ArgumentNullException("webElement");
            if (value == null) throw new ArgumentNullException("value");

            const int maxDigitals = 4;

            Actions actions = new Actions(_driver);
            actions.MoveToElement(webElement);
            actions.Click();

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.Backspace);

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.Delete);

            actions.SendKeys(value);
            actions.Build().Perform();
        }

        public void FindFlights()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IReadOnlyCollection<IWebElement> buttonsWebElements = _driver.FindElements(By.ClassName("btn-smart-search-go"));
            foreach (var btn in buttonsWebElements)
            {
                if (btn.Displayed)
                    btn.Click();
            }
        }

        public string GetInputValidationState()
        {
            IWebElement errorWebElement = null;

            Thread.Sleep(TimeSpan.FromSeconds(1));

            try
            {
                errorWebElement = _driver.FindElement(By.ClassName("ryanair-error-tooltip"));
            }
            catch
            {
                return "OK";
            }

            return errorWebElement.FindElement(By.TagName("span")).GetAttribute("innerHTML");
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            List<Flight> result = new List<Flight>();

            var flightSlides = _driver.FindElement(By.CssSelector("div[class='wrapper']")).FindElements(By.ClassName("slide"));

            foreach (var slide in flightSlides)
            {
                var flight = GetOneItemFromCarousel(slide, searchCriteria);

                if (flight != null)
                    result.Add(flight);
            }

            return result;
        } 

        public void TerminateSite()
        {
            //_driver.Close();
        }

        public bool IsNextPageLoadedSuccessfully()
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                
                _driver.FindElement(By.CssSelector("div[class='slide active']"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsInputWasFilledCorrectly(string text, IWebElement webElement)
        {
            var value = webElement.GetAttribute("value");

            if (string.IsNullOrEmpty(value))
                return false;

            return webElement.GetAttribute("value") == text;
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
                string dateLong = webElement.FindElement(By.ClassName("date")).GetAttribute("innerHTML");
                result.DepartureTime = _ryanAirDateConverter.Convert(searchCriteria.DepartureDate, dateLong);

                var res1 = DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(-2));
                var res2 = DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(2));

                if (DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(-2)) < 0 ||
                    DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(2)) > 0)
                    return null;

                var priceSlide = webElement.FindElement(By.ClassName("carousel-item"));
                string className = priceSlide.GetAttribute("class");

                switch (className)
                {
                    case "carousel-item daily item-not-available":
                        return null;
                    default:
                        result.SearchValidationText = "OK";
                        break;
                }

                var fareLong = webElement.FindElement(By.ClassName("fare")).GetAttribute("innerHTML");

                AddCurrency(ref result, fareLong);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public void AddCurrency(ref Flight flightToAddCurrency, string price)
        {
            price = price.Trim('\r', '\n', ' ');
            string[] priceArray = price.Split(new [] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);

            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = priceArray.Last()
            });
            flightToAddCurrency.Price = int.Parse(string.Join("", priceArray.Reverse().Skip(1).Reverse()), NumberStyles.Currency);
        }
    }
}
