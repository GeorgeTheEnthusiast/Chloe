using System;
using System.Collections.Generic;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using OpenQA.Selenium;

namespace Flights.FlightsControllers
{
    public class NorwegianFlightsNetController : IFlightsNetController
    {
        private readonly IWebDriver _driver;
        private readonly ICitiesCommand _citiesCommand;
        private readonly ICityQuery _cityQuery;
        private readonly INetCommand _netCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private Flights.Dto.FlightWebsite _flightWebsite;

        public NorwegianFlightsNetController(
            IWebDriver driver, 
            ICitiesCommand citiesCommand,
            ICityQuery cityQuery,
            INetCommand netCommand,
            IFlightWebsiteQuery flightWebsiteQuery
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (citiesCommand == null) throw new ArgumentNullException("citiesCommand");
            if (cityQuery == null) throw new ArgumentNullException("cityQuery");
            if (netCommand == null) throw new ArgumentNullException("netCommand");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");

            _driver = driver;
            _citiesCommand = citiesCommand;
            _cityQuery = cityQuery;
            _netCommand = netCommand;
            _flightWebsiteQuery = flightWebsiteQuery;
        }

        public void CreateNet()
        {
//            NavigateToUrl();
//
//            ExpandCitiesDropDownList();
//            
//            List<City> cities = GetAllCities();
//            List<City> citiesToRepeat = new List<City>();
//
//            while (cities.Count > 0)
//            {
//                foreach (var city in cities)
//                {
//                    try
//                    {
//                        FillCityFrom(city.Name);
//                        CreateNet(city);
//                        citiesToRepeat.Remove(city);
//                    }
//                    catch (Exception)
//                    {
//                        citiesToRepeat.Add(city);
//                    }
//                }
//
//                cities = citiesToRepeat.ToList();
//            }
        }

        private void NavigateToUrl()
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(Dto.Enums.FlightWebsite.Norwegian);

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
            fromCityWebElement.SendKeys(Keys.Backspace);
            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Tab);
        }

        private void FillCityTo(string cityName)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("input[placeholder='Dokąd chcesz się wybrać?']"));

            //toCityWebElement.Click();
            toCityWebElement.SendKeys(cityName);
            toCityWebElement.SendKeys(Keys.Enter);
        }
    }
}
