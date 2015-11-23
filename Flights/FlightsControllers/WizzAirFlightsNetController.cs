using System;
using System.Collections.Generic;
using System.Linq;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Flights.FlightsControllers
{
    public class WizzAirFlightsNetController : IFlightsNetController
    {
        private readonly IWebDriver _driver;
        private readonly ICitiesCommand _citiesCommand;
        private readonly ICityQuery _cityQuery;
        private readonly INetCommand _netCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierQuery _carrierQuery;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private Flights.Dto.Carrier _carrier;

        public WizzAirFlightsNetController(
            IWebDriver driver, 
            ICitiesCommand citiesCommand,
            ICityQuery cityQuery,
            INetCommand netCommand,
            IFlightWebsiteQuery flightWebsiteQuery,
            ICarrierQuery carrierQuery
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (citiesCommand == null) throw new ArgumentNullException("citiesCommand");
            if (cityQuery == null) throw new ArgumentNullException("cityQuery");
            if (netCommand == null) throw new ArgumentNullException("netCommand");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (carrierQuery == null) throw new ArgumentNullException("carrierQuery");

            _driver = driver;
            _citiesCommand = citiesCommand;
            _cityQuery = cityQuery;
            _netCommand = netCommand;
            _flightWebsiteQuery = flightWebsiteQuery;
            _carrierQuery = carrierQuery;
        }

        public void CreateNet()
        {
            return;

            NavigateToUrl();

            ExpandCountriesDropDownList();
            
            List<City> cities = GetAllCities();
            List<City> citiesToRepeat = new List<City>();

            while (cities.Count > 0)
            {
                foreach (var city in cities)
                {
                    try
                    {
                        FillCityFrom(city.Name);
                        CreateNet(city);
                        citiesToRepeat.Remove(city);
                    }
                    catch (Exception)
                    {
                        citiesToRepeat.Add(city);
                    }
                }

                cities = citiesToRepeat.ToList();
            }
        }

        private void NavigateToUrl()
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(Dto.Enums.FlightWebsite.WizzAir);

            if (_carrier == null)
                _carrier = _carrierQuery.GetCarrierByName("WizzAir");

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }
        
        private void ExpandCountriesDropDownList()
        {
            IWebElement webElement = _driver.FindElement(By.ClassName("city-from"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(webElement);

            actions.Click();
            actions.Build().Perform();
        }

        private List<City> GetAllCities()
        {
            List<City> result = new List<City>();
            IWebElement webElement = _driver.FindElement(By.CssSelector("div[class='box-autocomplete inHeader']"));
            var citiesWebElements =
                webElement.FindElements(By.TagName("li"));

            foreach (var cityWebElement in citiesWebElements)
            {
                City c = new City();
                c.Name = cityWebElement.GetAttribute("innerHTML")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "");
                c.Name = c.Name.Substring(0, c.Name.IndexOf('('))
                    .Trim();

                c = _citiesCommand.Merge(c);

                result.Add(c);
            }

            return result;
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.ClassName("city-from"));
            
            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(Keys.Backspace);
            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Tab);
        }

        private void CreateNet(City cityFrom)
        {
            IWebElement toCityWebElement = _driver.FindElements(By.CssSelector("div[class='box-autocomplete inHeader']"))[1];
            
            var toCitiesWebElements = toCityWebElement.FindElements(By.TagName("li"));

            foreach (var cityWebElement in toCitiesWebElements)
            {
                string cityToName = cityWebElement.GetAttribute("innerHTML")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "");
                cityToName = cityToName.Substring(0, cityToName.IndexOf('('))
                    .Trim();

                City cityTo = _cityQuery.GetCityByName(cityToName);
                Net net = new Net()
                {
                    Carrier = _carrier,
                    CityFrom = cityFrom,
                    CityTo = cityTo
                };

                _netCommand.Merge(net);
            }
        }
    }
}
