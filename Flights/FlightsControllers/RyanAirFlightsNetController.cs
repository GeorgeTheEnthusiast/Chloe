using System;
using System.Collections.Generic;
using System.Linq;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Flights.FlightsControllers
{
    public class RyanAirFlightsNetController : IFlightsNetController
    {
        private readonly IWebDriver _driver;
        private readonly ICitiesCommand _citiesCommand;
        private readonly ICityQuery _cityQuery;
        private readonly INetCommand _netCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierQuery _carrierQuery;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private Flights.Dto.Carrier _carrier;
        private WebDriverWait _webDriverWait;

        public RyanAirFlightsNetController(
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
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
        }

        public void CreateNet()
        {
            return;

            NavigateToUrl();

            ExpandCountriesDropDownList();

            SelectAllCountries();

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
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(Dto.Enums.FlightWebsite.RyanAir);

            if (_carrier == null)
                _carrier = _carrierQuery.GetCarrierByName("RyanAir");

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }
        
        private void ExpandCountriesDropDownList()
        {
            IWebElement webElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-from']"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(webElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.Build().Perform();
        }

        private void SelectAllCountries()
        {
            IWebElement webElement = _driver.FindElement(By.CssSelector("div[ng-class='{clicked: ll.allCountriesFirstSelected}']"));
            webElement.Click();

            Actions actions = new Actions(_driver);
            actions.MoveToElement(webElement);

            actions.Click();
            actions.Build().Perform();
        }

        private List<City> GetAllCities()
        {
            List<City> result = new List<City>();
            IWebElement webElement = _driver.FindElement(By.CssSelector("div[class='pane right']"));
            var citiesWebElements =
                webElement.FindElements(By.CssSelector("div[ng-repeat='option in ll.secondOptions track by option.id']"));

            foreach (var cityWebElement in citiesWebElements)
            {
                City c = new City();
                c.Name = cityWebElement.GetAttribute("innerHTML").Trim();

                c = _citiesCommand.Merge(c);

                result.Add(c);
            }

            return result;
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-from']"));
            int maxDigitals = 30;

            Actions actions = new Actions(_driver);
            actions.MoveToElement(fromCityWebElement);

            actions.Click();

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.Backspace);

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.Delete);
            
            actions.SendKeys(cityName);
            actions.SendKeys(Keys.Tab);
            actions.Build().Perform();
            
            if (IsInputWasFilledCorrectly(cityName, fromCityWebElement) == false)
                throw new InputWasNotFilledCorrectlyException();
        }

        private bool IsInputWasFilledCorrectly(string text, IWebElement webElement)
        {
            var value = webElement.GetAttribute("value");

            if (string.IsNullOrEmpty(value))
                return false;

            return webElement.GetAttribute("value") == text;
        }

        private void CreateNet(City cityFrom)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-to']"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(toCityWebElement);

            actions.Click();
            actions.Build().Perform();

            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-from']"));
            
            actions.MoveToElement(fromCityWebElement);

            actions.Click();
            actions.Build().Perform();

            actions.MoveToElement(toCityWebElement);

            actions.Click();
            actions.Build().Perform();

            IWebElement paneLeftWebElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='pane left']")));
            IWebElement paneRightWebElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='pane right']")));

            IWait<IWebElement> _waitForCountries = new DefaultWait<IWebElement>(paneLeftWebElement);
            _waitForCountries.Timeout = TimeSpan.FromSeconds(2);
            var countriesWebELement = paneLeftWebElement.FindElements(By.CssSelector("div[class='option']"));

            foreach (var countryWebElement in countriesWebELement)
            {
                Actions action = new Actions(_driver);
                action.MoveToElement(countryWebElement);

                action.Click();
                action.Build().Perform();

                IWait<IWebElement> _waitForCities = new DefaultWait<IWebElement>(paneRightWebElement);
                _waitForCities.Timeout = TimeSpan.FromSeconds(2);

                var citiesWebElement = paneRightWebElement.FindElements(By.CssSelector("div[class='option']"));

                foreach (var cityWebElement in citiesWebElement)
                {
                    string cityWebElement_Name = cityWebElement.GetAttribute("innerHTML").Trim();
                    City cityTo = _cityQuery.GetCityByName(cityWebElement_Name);
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
}
