using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;

namespace Flights.Controllers.FlightsControllers
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
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(20));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }
        
        private void ExpandCountriesDropDownList()
        {
            IWebElement webElement = _driver.FindElement(By.Id("search-departure-station"));

            webElement.Click();
        }

        private List<City> GetAllCities()
        {
            List<City> result = new List<City>();
            var scroll = _driver.FindElement(By.ClassName("ps-scrollbar-y"));
            IWebElement webElement = _driver.FindElement(By.ClassName("flight-search__panel__loader"));
            var citiesWebElements =
                webElement.FindElements(By.TagName("label"));
            int index = -1;

            foreach (var cityWebElement in citiesWebElements)
            {
                if (index == -1)
                    cityWebElement.Click();

                if (index == 17)
                {
                    index = 0;
                    ScrollCityListToElement(scroll, cityWebElement);
                }

                City c = new City();
                c.Name = cityWebElement.FindElement(By.TagName("strong")).Text.Trim();
                c.Alias = cityWebElement.FindElement(By.TagName("small")).Text.Trim();

                if (c.Name == string.Empty)
                {
                    if (cityWebElement.Displayed == false)
                    {
                        scroll.SendKeys(Keys.PageDown);
                    }
                    c.Name = cityWebElement.FindElement(By.TagName("strong")).Text.Trim();
                    var text = cityWebElement.Text;
                }

                //TODO na potrzeby prezentacji c = _citiesCommand.Merge(c);

                if (c.Name != string.Empty)
                {
                    result.Add(c);
                }

                index++;
            }

            return result;
        }

        private void ScrollCityListToElement(IWebElement scrollElement, IWebElement cityElement)
        {
            while (cityElement.Location.Y > 120)
            {
                scrollElement.SendKeys(Keys.PageDown);
            }
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.Id("search-departure-station"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(Keys.Backspace);
            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Tab);
        }

        private void CreateNet(City cityFrom)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.Id("search-arrival-station"));
            toCityWebElement.Click();

            IWebElement webElement = _driver.FindElement(By.ClassName("flight-search__panel__loader"));
            var toCitiesWebElements =
                webElement.FindElements(By.TagName("label"));

            foreach (var cityWebElement in toCitiesWebElements)
            {
                string cityToName = cityWebElement.FindElement(By.TagName("strong")).Text.Trim();

                City cityTo = _cityQuery.GetCityByName(cityToName);
                Net net = new Net()
                {
                    Carrier = _carrier,
                    CityFrom = cityFrom,
                    CityTo = cityTo
                };

                //TODO na potrzeby prezentacji _netCommand.Merge(net);
            }
        }

        private void ScrollPageDown()
        {
            try
            {
                _driver.ExecuteJavaScript<IWebElement>("scroll(0, 400)");
            }
            catch
            {
            }
        }
    }
}
