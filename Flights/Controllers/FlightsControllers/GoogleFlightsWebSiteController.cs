using System;
using System.Collections.Generic;
using System.Globalization;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Flights.Controllers.FlightsControllers
{
    public class GoogleFlightsWebSiteController : IWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierCommand _carrierCommand;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;

        public GoogleFlightsWebSiteController(IWebDriver driver,
            ICurrienciesCommand currienciesCommand,
            IFlightWebsiteQuery flightWebsiteQuery,
            ICarrierCommand carrierCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (carrierCommand == null) throw new ArgumentNullException("carrierCommand");

            _driver = driver;
            _currienciesCommand = currienciesCommand;
            _flightWebsiteQuery = flightWebsiteQuery;
            _carrierCommand = carrierCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }

        private void FillCityFrom(string name)
        {
            var fromCityWebElementInput = _driver.FindElements(By.TagName("input"))[1];
            var fromCityWebElementClick = fromCityWebElementInput.FindElement(By.XPath(".."));

            fromCityWebElementClick.Click();
            fromCityWebElementInput.SendKeys(name);
            fromCityWebElementInput.SendKeys(Keys.Enter);
        }

        private void FillCityTo(string name)
        {
            var toCityWebElementInput = _driver.FindElements(By.TagName("input"))[2];
            var toCityWebElementClick = toCityWebElementInput.FindElement(By.XPath(".."));

            toCityWebElementClick.Click();
            toCityWebElementInput.SendKeys(name);
            toCityWebElementInput.SendKeys(Keys.Enter);
        }

        private void FillDate(DateTime departureDate)
        {
            var datePickerWebElementInput = _driver.FindElements(By.TagName("input"))[3];
            var datePickerWebElementClick = datePickerWebElementInput.FindElement(By.XPath(".."));

            datePickerWebElementClick.Click();
            datePickerWebElementInput.SendKeys(departureDate.ToString("dd.MM.yyyy"));
            datePickerWebElementInput.SendKeys(Keys.Enter);
        }

        private void MakeFlightOneWay()
        {
            var oneWayTicketWebElement = _driver.FindElement(By.XPath("/html/body/div[1]/div[3]/table/tbody/tr[2]/td/table/tbody/tr/td[2]/div/div/div[1]/div[3]/button[2]"));

            oneWayTicketWebElement.Click();
        }

        private Flight GetTheBestFlight(SearchCriteria searchCriteria, DateTime departureDate)
        {
            try
            {
//                var flightIsNotAvailableWebElement =
//                _driver.FindElement(
//                    By.XPath(
//                        "/html/body/div[1]/div[3]/table/tbody/tr[2]/td/table/tbody/tr/td[2]/div/div/div[3]/div[1]/div/div[2]/div[2]/div[2]/div[1]/div"));

                var flightIsNotAvailableWebElement =
                    _driver.FindElement(By.CssSelector("div[value='Nie udało się znaleźć takich lotów.']"));

                return null;
//
//                if (flightIsNotAvailableWebElement.Text == "Nie udało się znaleźć takich lotów.")
//                    return null;
            }
            catch (Exception ex)
            {
                
            }

            IWebElement flightWebElement =
                _driver.FindElement(
                    By.XPath(
                        "/html/body/div[1]/div[3]/table/tbody/tr[2]/td/table/tbody/tr/td[2]/div/div/div[3]/div[1]/div/div[2]/div[2]/div[1]"));

            try
            {
                flightWebElement = flightWebElement.FindElement(By.XPath("div[2]/a"));
            }
            catch (Exception ex)
            {
                try
                {
                    flightWebElement = flightWebElement.FindElement(By.XPath("div[1]/a"));
                }
                catch (Exception e)
                {
                    _logger.Error("I tried 2 times to retrieve flight, but with no luck :/");
                    return null;
                }
            }
            
            Flight result = new Flight()
            {
                SearchDate = DateTime.Now,
                SearchCriteria = searchCriteria
            };
            
            var priceWebElement = flightWebElement.FindElement(By.XPath("div[1]"));
            var carrierWebElement = flightWebElement.FindElement(By.XPath("div[2]"));
            var isDirectWebElement = flightWebElement.FindElement(By.XPath("div[4]"));

            result.IsDirect = IsFlightDirect(isDirectWebElement);
            result.Price = GetPrice(priceWebElement);
            result.Carrier = GetCarrier(carrierWebElement);
            result.DepartureTime = GetDepartureDate(carrierWebElement, departureDate);
            result.Currency = GetCurrency();

            return result;
        }

        private decimal GetPrice(IWebElement webElement)
        {
            var priceElement = webElement.FindElement(By.XPath("div[1]/div[1]"));
            string valueToParse = priceElement.Text
                .Replace("zł", "")
                .Replace(" ", "")
                .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            decimal result = 0;
            if (!decimal.TryParse(valueToParse, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                _logger.Info("Could not parse price value: " + valueToParse);
                throw new PriceIsEmptyException();
            }
            return result;
        }

        private DateTime GetDepartureDate(IWebElement webElement, DateTime departureDate)
        {
            var carrierElement = webElement.FindElement(By.XPath("div[1]/span[1]"));
            string dateToParse = string.Format("{0} {1}:00", departureDate.ToString("dd-MM-yyyy"), carrierElement.Text);
            var result = DateTime.Parse(dateToParse);

            return result;
        }

        private Currency GetCurrency()
        {
            var result = _currienciesCommand.Merge(new Currency()
            {
                Name = "zł"
            });

            return result;
        }

        private Carrier GetCarrier(IWebElement webElement)
        {
            var carrierElement = webElement.FindElement(By.XPath("div[2]/span[1]"));
            string carrierToMerge = carrierElement.Text;

            if (carrierToMerge.Contains(","))
            {
                carrierToMerge = carrierToMerge.Substring(0, carrierToMerge.IndexOf(','));
            }

            var result = _carrierCommand.Merge(carrierToMerge);

            return result;
        }

        private bool IsFlightDirect(IWebElement webElement)
        {
            var directWebElement = webElement.FindElement(By.TagName("div"));
            return directWebElement.Text == "Lot bezpośredni";
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(Dto.Enums.FlightWebsite.GoogleFlights);

            List<Flight> result = new List<Flight>();

            if (searchCriteria.FlightWebsite.Id != _flightWebsite.Id)
                return result;
            
            for (DateTime day = searchCriteria.DepartureDate.AddDays(-2);
                DateTime.Compare(day, searchCriteria.DepartureDate.AddDays(2)) != 0;
                day = day.AddDays(1))
            {
                NavigateToUrl();

                MakeFlightOneWay();

                FillCityFrom(searchCriteria.CityFrom.Alias);

                FillCityTo(searchCriteria.CityTo.Alias);

                FillDate(day);
                
                var flight = GetTheBestFlight(searchCriteria, day);

                if (flight != null)
                    result.Add(flight);
            }

            return result;
        }
    }
}
