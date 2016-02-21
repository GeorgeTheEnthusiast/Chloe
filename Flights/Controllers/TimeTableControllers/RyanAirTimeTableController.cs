using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Flights.Dto.Enums.FlightWebsite;

namespace Flights.Controllers.TimeTableControllers
{
    public class RyanAirTimeTableController : ITimeTableController
    {
        private readonly ITimeTableCommand _timeTableCommand;
        private readonly ICitiesCommand _citiesCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICityQuery _cityQuery;
        private readonly ICarrierQuery _carrierQuery;
        private readonly ITimeTableStatusCommand _timeTableStatusCommand;
        private readonly IWebDriver _driver;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;
        private Carrier _carrier;

        private By _searchByCityFrom;
        private By _searchByCityTo;

        public RyanAirTimeTableController(IWebDriver driver,
            ITimeTableCommand timeTableCommand,
            ICitiesCommand citiesCommand,
            IFlightWebsiteQuery flightWebsiteQuery,
            ICityQuery cityQuery,
            ICarrierQuery carrierQuery,
            ITimeTableStatusCommand timeTableStatusCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (timeTableCommand == null) throw new ArgumentNullException("timeTableCommand");
            if (citiesCommand == null) throw new ArgumentNullException("citiesCommand");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (cityQuery == null) throw new ArgumentNullException("cityQuery");
            if (carrierQuery == null) throw new ArgumentNullException("carrierQuery");
            if (timeTableStatusCommand == null) throw new ArgumentNullException("timeTableStatusCommand");

            _driver = driver;
            _timeTableCommand = timeTableCommand;
            _citiesCommand = citiesCommand;
            _flightWebsiteQuery = flightWebsiteQuery;
            _cityQuery = cityQuery;
            _carrierQuery = carrierQuery;
            _timeTableStatusCommand = timeTableStatusCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.RyanAirAirpot);
            _carrier = _carrierQuery.GetCarrierByName("RyanAir");
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(12));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }
        
        public void CreateTimeTable()
        {
            _searchByCityFrom = By.CssSelector("input[placeholder='Lotnisko wylotu:']");
            _searchByCityTo = By.CssSelector("input[placeholder='Lotnisko docelowe:']");
            var searchBySearchButton = By.CssSelector("button[class='core-btn-primary core-btn-big core-btn-block']");

            NavigateToUrl();

            ExpandAllCities();
            
            var timeTableStatusList = CreateTimeTableStatusList();
            var timeTableStatusListToRepeat = timeTableStatusList.ToList();

            RestartSite(searchBySearchButton);

            Stopwatch counter = new Stopwatch();
            counter.Start();

            while (timeTableStatusList.Count > 0)
            {
                foreach (var timeTableStatus in timeTableStatusList)
                {
                    if (counter.Elapsed.TotalMinutes >= 18)
                    {
                        _logger.Debug("Resetting the counter, restarting the website...");
                        counter.Restart();
                        RestartSite(searchBySearchButton);
                    }

                    var searchButtonWebElement = _driver.FindElement(searchBySearchButton);

                    FillCity(_searchByCityFrom, timeTableStatus.CityFrom.Name);
                    FillCity(_searchByCityTo, timeTableStatus.CityTo.Name);
                    timeTableStatus.SearchDate = DateTime.Now;
                    searchButtonWebElement.Click();
                
                    ReadData(timeTableStatus, ref timeTableStatusListToRepeat);
                
                    _logger.Info("TimeTables left: [{0}]", timeTableStatusListToRepeat.Count);
                }

                timeTableStatusList = timeTableStatusListToRepeat.ToList();
            }

            counter.Stop();
        }

        private void RestartSite(By searchBy)
        {
            NavigateToUrl();
            _webDriverWait.Until(x => x.FindElement(searchBy));
        }

        private void ReadData(TimeTableStatus timeTableStatus, ref List<TimeTableStatus> timeTableStatusList)
        {
            try
            {
                ReadAllMonthsTimeTable(timeTableStatus.CityFrom, timeTableStatus.CityTo);
                
                _logger.Debug("Adding timeTable from city [{0}] to [{1}] completed without errors.",
                    timeTableStatus.CityFrom.Name, timeTableStatus.CityTo.Name);

                timeTableStatusList.RemoveAll(
                    x =>
                        x.CityFrom.Id == timeTableStatus.CityFrom.Id &&
                        x.CityTo.Id == timeTableStatus.CityTo.Id);

                _timeTableStatusCommand.Merge(timeTableStatus);
            }
            catch (Exception ex)
            {
                _logger.Error("I have to repeat timeTable from [{0}] to [{1}]", timeTableStatus.CityFrom.Name,
                    timeTableStatus.CityTo.Name);
                _logger.Error(ex);
            }
        }

        private List<TimeTableStatus> CreateTimeTableStatusList()
        {
            var cities = GetAllCities();
            var timeTableStatusList = new List<TimeTableStatus>();
            int i = 0;

            foreach (var city in cities)
            {
                _logger.Info("Getting destination citites [{0}]/[{1}]...", i, cities.Count);
                FillCity(_searchByCityFrom, city.Name);
                var citiesTo = GetAllCitiesTo();

                foreach (var cityTo in citiesTo)
                {
                    var timeTableStatus = new TimeTableStatus()
                    {
                        CityFrom = city,
                        CityTo = cityTo,
                        FlightWebsite = _flightWebsite,
                        SearchDate = null
                    };
                    timeTableStatus = _timeTableStatusCommand.Merge(timeTableStatus);
                    timeTableStatusList.Add(timeTableStatus);
                }

                i++;
            }

            timeTableStatusList =
                timeTableStatusList.Where(
                    x =>
                        x.SearchDate == null ||
                        DateTime.Compare(x.SearchDate.Value.Date, DateTime.Now.AddMonths(-3).Date) == -1)
                    .ToList();

            return timeTableStatusList;
        }
        
        private void ExpandAllCities()
        {
            IWebElement webElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("input[placeholder='Lotnisko wylotu:']")));
            webElement.Click();
        }

        private List<City> GetAllCities()
        {
            List<City> result = new List<City>();
            int i = 0;
            IWebElement webElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='three-cols']")));

            var countriesWebElements =
                webElement.FindElements(By.CssSelector("div[class='option']"));

            foreach (var countryWebElement in countriesWebElements)
            {
                _logger.Info("Processing [{0}]/[{1}] cities", i, countriesWebElements.Count);
                countryWebElement.Click();

                var citiesWebElements = _driver
                    .FindElement(By.CssSelector("div[class='pane right']"))
                    .FindElements(By.CssSelector("div[class='option']"));
                
                foreach (var cityWebElement in citiesWebElements)
                {
                    City city = new City();

                    city.Name = cityWebElement.Text.Trim();
                    city = _citiesCommand.Merge(city);

                    result.Add(city);
                }

                i++;
            }

            return result;
        }

        private List<City> GetAllCitiesTo()
        {
            List<City> result = new List<City>();

            IWebElement webElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='three-cols']")));

            var countriesWebElements =
                webElement.FindElements(By.CssSelector("div[class='option']"));

            foreach (var countryWebElement in countriesWebElements)
            {
                countryWebElement.Click();

                var citiesWebElements = _driver
                    .FindElement(By.CssSelector("div[class='pane right']"))
                    .FindElements(By.CssSelector("div[class='option']"));

                foreach (var cityWebElement in citiesWebElements)
                {
                    City city = new City();

                    city.Name = cityWebElement.Text.Trim();
                    city = _cityQuery.GetCityByName(city.Name);

                    result.Add(city);
                }
            }

            return result;
        }

        private void FillCity(By searchBy, string cityName)
        {
            IWebElement cityWebElement = _driver.FindElement(searchBy);

            cityWebElement.Click();

            for (int i = 0; i < 30; i++)
            {
                cityWebElement.SendKeys(Keys.Backspace);
            }

            cityWebElement.SendKeys(cityName);

            for (int i = 0; i < 30; i++)
            {
                cityWebElement.SendKeys(Keys.Delete);
            }

            cityWebElement.SendKeys(Keys.Enter);
            cityWebElement.SendKeys(Keys.Tab);
        }
        
        private TimeSpan GetTimeDifference(TimeSpan departureTime, TimeSpan arrivalTime)
        {
            TimeSpan timeDifference;
            if (TimeSpan.Compare(departureTime, arrivalTime) == -1)
            {
                timeDifference = arrivalTime.Subtract(departureTime);
            }
            else
            {
                timeDifference = new TimeSpan(0, 0, 0);
                TimeSpan timeToMidnight = (new TimeSpan(24, 0, 0)).Subtract(departureTime);
                TimeSpan timeFromMidnight = arrivalTime;
                timeDifference = timeDifference.Add(timeToMidnight);
                timeDifference = timeDifference.Add(timeFromMidnight);
            }

            return timeDifference;
        }
        
        private void ReadCurrentMonthTimeTable(City cityFrom, City cityTo, By searchByCalendar)
        {
            var searchByFlight = By.CssSelector("li[class='flight']");
            var departuresWebElement = _webDriverWait.Until(x => x.FindElement(searchByCalendar));
            var cellsWebElement = departuresWebElement
                .FindElements(By.ClassName("cell"))
                .ToList();
            var cellsLinq = (from c in cellsWebElement.Where(x => x.FindElements(searchByFlight).Count > 0)
                             let date = c.GetAttribute("date-id")
                let flights =
                    (
                        from f in c.FindElements(searchByFlight)
                        select new
                        {
                            DepartureTimeString = f.FindElement(By.CssSelector("span[class='departure-time']")).Text,
                            ArrivalTimeString = f.FindElement(By.CssSelector("span[class='arrival-time']")).Text
                        }
                        )
                select new {Date = date, Flights = flights}).ToList();


            foreach (var cell in cellsLinq)
            {
                foreach (var flight in cell.Flights)
                {
                    DateTime departureDate = DateTime.ParseExact(cell.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    if (DateTime.Compare(departureDate.Date, DateTime.Now.AddYears(1)) >= 0)
                        continue;

                    TimeSpan departureTime = TimeSpan.Parse(flight.DepartureTimeString);
                    TimeSpan arrivalTime = TimeSpan.Parse(flight.ArrivalTimeString);
                    TimeSpan timeDifference = GetTimeDifference(departureTime, arrivalTime);
                    TimeTable timeTable = new TimeTable()
                    {
                        FlightWebsite = _flightWebsite,
                        Carrier = _carrier,
                        CityFrom = cityFrom,
                        CityTo = cityTo,
                        DepartureDate = departureDate.AddMinutes(departureTime.TotalMinutes)
                    };
                    timeTable.ArrivalDate = timeTable.DepartureDate.AddMinutes(timeDifference.TotalMinutes);

                    _timeTableCommand.Merge(timeTable);
                }
            }
        }

        private void ReadAllMonthsTimeTable(City cityFrom, City cityTo)
        {
            var searchByDepartures = By.CssSelector("div[route='outbound']");
            var currentCalendar = _webDriverWait.Until(x => x.FindElement(searchByDepartures));
            var scroller = currentCalendar.FindElement(By.CssSelector("ul[class='scroller']"));
            Dictionary<string, bool> monthsReadStatus = new Dictionary<string, bool>();
            bool readMore = true;

            while (readMore)
            {
                var monthsLinq = (from m in scroller.FindElements(By.ClassName("item"))
                    select new {WebElement = m, DictionaryKey = m.Text}).ToList();

                var month = monthsLinq.FirstOrDefault(x => (string.IsNullOrEmpty(x.DictionaryKey) == false) && (monthsReadStatus.ContainsKey(x.DictionaryKey) == false
                || monthsReadStatus[x.DictionaryKey] == false) );

                if (month == null || monthsReadStatus.ContainsKey(month.DictionaryKey) && monthsReadStatus[month.DictionaryKey] == true)
                    continue;

                if (month.WebElement.Displayed)
                {
                    month.WebElement.Click();
                    ReadCurrentMonthTimeTable(cityFrom, cityTo, searchByDepartures);
                    monthsReadStatus[month.DictionaryKey] = true;
                }
                else
                {
                    monthsReadStatus[month.DictionaryKey] = false;
                }

                ScrollCarouselRight();

                if (monthsReadStatus.Count == monthsLinq.Count
                    && monthsReadStatus.All(x => x.Value == true))
                    readMore = false;
            }
        }

        private void ScrollCarouselRight()
        {
            var outboundWebElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[type='outbound']")));
            var nextButtons = outboundWebElement.FindElements(By.CssSelector("button[class='arrow right']"));

            if (nextButtons.Count > 0 && nextButtons[0].Displayed)
            {
                nextButtons[0].Click();
            }
        }
    }

    
}
