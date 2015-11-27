using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Flights.FlightsControllers
{
    public class NorwegianFlightsNetController : IFlightsNetController
    {
        private readonly IWebDriver _driver;
        private readonly ICitiesCommand _citiesCommand;
        private readonly ICityQuery _cityQuery;
        private readonly INetCommand _netCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierQuery _carrierQuery;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private Flights.Dto.Carrier _carrier;

        public NorwegianFlightsNetController(
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

            ExpandCitiesDropDownList();
            
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
                        CloseCityToDropDownList();
                    }
                    catch (Exception ex)
                    {
                        CloseCityToDropDownList();
                        citiesToRepeat.Add(city);
                    }
                }

                cities = citiesToRepeat.ToList();
            }
        }

        private void NavigateToUrl()
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(Dto.Enums.FlightWebsite.Norwegian);

            if (_carrier == null)
                _carrier = _carrierQuery.GetCarrierByName("Norwegian");

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }
        
        private void ExpandCitiesDropDownList()
        {
            IWebElement webElement = _driver.FindElement(By.CssSelector("input[placeholder='Twój punkt wyjścia']"));

            webElement.Click();
        }

        private List<City> GetAllCities()
        {
            List<City> result = new List<City>();
            IWebElement webElement = _driver.FindElement(By.CssSelector("div[data-ng-model='model.request.origin']"));
            var citiesWebElements =
                webElement.FindElements(By.TagName("li"));

            foreach (var cityWebElement in citiesWebElements)
            {
                City c = new City();
                
                c.Name = cityWebElement
                    .FindElement(By.TagName("strong"))
                    .Text;

                c.Alias = c.Name.Substring(c.Name.IndexOf('(') + 1, 3);

                c.Name = c.Name.Substring(0, c.Name.IndexOf('('))
                    .Trim();

                c = _citiesCommand.Merge(c);

                result.Add(c);
            }

            return result;
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("input[placeholder='Twój punkt wyjścia']"));

            fromCityWebElement.Click();

            for (int i = 0; i < 40; i++)
            {
                fromCityWebElement.SendKeys(Keys.Backspace);
            }

            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Tab);
        }
        
        private void CreateNet(City cityFrom)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement toCityWebElement =
                _driver.FindElements(By.CssSelector("section[class='form-split__item']"))[1];

            var toCitiesWebElements = toCityWebElement.FindElements(By.TagName("li"));

            foreach (var cityWebElement in toCitiesWebElements)
            {
                string cityToName = cityWebElement
                    .FindElement(By.TagName("strong"))
                    .Text;

                if (cityToName.Contains("("))
                    cityToName = cityToName.Substring(0, cityToName.IndexOf('('))
                        .Trim();
                else
                {
                    
                }
                
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

        private void CloseCityToDropDownList()
        {
            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("input[placeholder='Dokąd chcesz się wybrać?']"));
            toCityWebElement.Click();

            Actions actions = new Actions(_driver);
            actions.MoveToElement(toCityWebElement)
                .MoveByOffset(0, -100)
                .Click();
            actions.Build().Perform();
        }
    }
}
