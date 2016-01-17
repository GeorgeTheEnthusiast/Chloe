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
            
            while (timeTableStatusList.Count > 0)
            {
                foreach (var timeTableStatus in timeTableStatusList)
                {
                    var searchButtonWebElement = _driver.FindElement(searchBySearchButton);

                    FillCity(_searchByCityFrom, timeTableStatus.TimeTableStatus.CityFrom.Name);
                    FillCity(_searchByCityTo, timeTableStatus.TimeTableStatus.CityTo.Name);
                    timeTableStatus.TimeTableStatus.SearchDate = DateTime.Now;
                    searchButtonWebElement.Click();
                
                    ReadData(timeTableStatus, ref timeTableStatusListToRepeat);
                
                    _logger.Info("TimeTables left: [{0}]", timeTableStatusListToRepeat.Count);
                }

                timeTableStatusList = timeTableStatusListToRepeat.ToList();
            }
        }

        private void ReadData(TimeTableStatusHelper timeTableStatus, ref List<TimeTableStatusHelper> timeTableStatusList)
        {
            var searchByDepartures = By.CssSelector("div[route='outbound']");
            var searchByArrivals = By.CssSelector("div[route='inbound']");

            try
            {//przeijanie w prawo przewija nie ten kalendarz co potrzeba
                if (timeTableStatus.IsDeparture)
                {
                    ReadAllMonthsTimeTable(timeTableStatus.TimeTableStatus.CityFrom, timeTableStatus.TimeTableStatus.CityTo,
                        searchByDepartures);
                }
                else
                {
                    ReadAllMonthsTimeTable(timeTableStatus.TimeTableStatus.CityFrom, timeTableStatus.TimeTableStatus.CityTo,
                        searchByArrivals);
                }

                _logger.Debug("Adding timeTable from city [{0}] to [{1}] completed without errors.",
                    timeTableStatus.TimeTableStatus.CityFrom.Name, timeTableStatus.TimeTableStatus.CityTo.Name);

                timeTableStatusList.RemoveAll(
                    x =>
                        x.TimeTableStatus.CityFrom.Id == timeTableStatus.TimeTableStatus.CityFrom.Id &&
                        x.TimeTableStatus.CityTo.Id == timeTableStatus.TimeTableStatus.CityTo.Id);

                _timeTableStatusCommand.Merge(timeTableStatus.TimeTableStatus);
            }
            catch (Exception ex)
            {
                _logger.Error("I have to repeat timeTable from [{0}] to [{1}]", timeTableStatus.TimeTableStatus.CityFrom.Name,
                    timeTableStatus.TimeTableStatus.CityTo.Name);
                _logger.Error(ex);
            }
        }

        private List<TimeTableStatusHelper> CreateTimeTableStatusList()
        {
            var cities = GetAllCities();
            HashSet<TimeTableStatusHelper> timeTableStatusHelperHashSet = new HashSet<TimeTableStatusHelper>(new TimeTableStatusHelperComparer());
            var timeTableStatusList = new List<TimeTableStatusHelper>();

            foreach (var city in cities)
            {
                FillCity(_searchByCityFrom, city.Name);
                var citiesTo = GetAllCitiesTo();

                foreach (var cityTo in citiesTo)
                {
                    var timeTableStatusDeparture = new TimeTableStatus()
                    {
                        CityFrom = city,
                        CityTo = cityTo,
                        FlightWebsite = _flightWebsite,
                        SearchDate = null
                    };
                    var timeTableStatusArrival = new TimeTableStatus()
                    {
                        CityFrom = cityTo,
                        CityTo = city,
                        FlightWebsite = _flightWebsite,
                        SearchDate = null
                    };

                    //var mergeResult = _timeTableStatusCommand.Merge(timeTableStatusDeparture);
                    var timetableStatusHelperDeparture = new TimeTableStatusHelper()
                    {
                        TimeTableStatus = timeTableStatusDeparture,
                        IsDeparture = true
                    };
                    timeTableStatusHelperHashSet.Add(timetableStatusHelperDeparture);

                    //mergeResult = _timeTableStatusCommand.Merge(timeTableStatusArrival);
                    var timetableStatusHelperArrival = new TimeTableStatusHelper()
                    {
                        TimeTableStatus = timeTableStatusArrival,
                        IsDeparture = false
                    };
                    timeTableStatusHelperHashSet.Add(timetableStatusHelperArrival);
                }
            }

            timeTableStatusList =
                timeTableStatusHelperHashSet.Where(
                    x =>
                        x.TimeTableStatus.SearchDate == null ||
                        DateTime.Compare(x.TimeTableStatus.SearchDate.Value.Date, DateTime.Now.AddMonths(-3).Date) == -1)
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
            
            IWebElement webElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='three-cols']")));

            var countriesWebElements =
                webElement.FindElements(By.CssSelector("div[class='option']"));

            foreach (var countryWebElement in countriesWebElements)
            {
                countryWebElement.Click();

                var citiesWebElements = _driver
                    .FindElement(By.CssSelector("div[class='pane right']"))
                    .FindElements(By.CssSelector("div[class='option']"));

                _logger.Info("Found {0} cities", citiesWebElements.Count);

                foreach (var cityWebElement in citiesWebElements)
                {
                    City city = new City();

                    city.Name = cityWebElement.Text.Trim();
                    city = _citiesCommand.Merge(city);

                    result.Add(city);
                }
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
            var departuresWebElement = _webDriverWait.Until(x => x.FindElement(searchByCalendar));
            var cellsWebElement = departuresWebElement.FindElements(By.ClassName("cell"));

            foreach (var cell in cellsWebElement)
            {
                string date = cell.GetAttribute("date-id");
                var flights = cell.FindElements(By.CssSelector("li[class='flight']"));

                foreach (var flight in flights)
                {
                    string departureTimeString = flight.FindElement(By.CssSelector("span[class='departure-time']")).Text;
                    string arrivalTimeString = flight.FindElement(By.CssSelector("span[class='arrival-time']")).Text;
                    DateTime departureDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    if (DateTime.Compare(departureDate.Date, DateTime.Now.AddYears(1)) >= 0)
                        continue;

                    TimeSpan departureTime = TimeSpan.Parse(departureTimeString);
                    TimeSpan arrivalTime = TimeSpan.Parse(arrivalTimeString);
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

        private void ReadAllMonthsTimeTable(City cityFrom, City cityTo, By searchByCalendar)
        {
            var currentCalendar = _webDriverWait.Until(x => x.FindElement(searchByCalendar));
            var scroller = currentCalendar.FindElement(By.CssSelector("ul[class='scroller']"));
            var months = scroller.FindElements(By.ClassName("item"));
            Dictionary<string, bool> monthsReadStatus = new Dictionary<string, bool>();

            while (monthsReadStatus.Any(x => x.Value == false) || monthsReadStatus.Count == 0)
            {
                foreach (var month in months)
                {
                    string yearDisplay = month.FindElement(By.ClassName("year")).Text;
                    string monthDisplay = month.FindElement(By.ClassName("month")).Text;
                    string dictionaryKey = yearDisplay + monthDisplay;

                    if (string.IsNullOrEmpty(dictionaryKey))
                        continue;

                    if (monthsReadStatus.ContainsKey(dictionaryKey) && monthsReadStatus[dictionaryKey] == true)
                        continue;

                    if (month.Displayed)
                    {
                        month.Click();
                        ReadCurrentMonthTimeTable(cityFrom, cityTo, searchByCalendar);
                        monthsReadStatus[dictionaryKey] = true;
                    }
                    else
                    {
                        monthsReadStatus[dictionaryKey] = false;
                    }

                    ScrollCarouselRight(scroller);
                }

                currentCalendar = _webDriverWait.Until(x => x.FindElement(searchByCalendar));
                scroller = currentCalendar.FindElement(By.CssSelector("ul[class='scroller']"));
                months = scroller.FindElements(By.ClassName("item"));
            }
        }

        private void ScrollCarouselRight(IWebElement currentScroller)
        {
            var nextButton = _webDriverWait.Until(x => x.FindElement(By.CssSelector("button[class='arrow right']")));

            if (nextButton.Displayed)
            {
                nextButton.Click();
            }
        }

        private class TimeTableStatusHelper
        {
            public TimeTableStatus TimeTableStatus;
            public bool IsDeparture;
        }

        private class TimeTableStatusHelperComparer : IEqualityComparer<TimeTableStatusHelper>
        {
            public bool Equals(TimeTableStatusHelper x, TimeTableStatusHelper y)
            {
                bool IdsAreEqual = x.TimeTableStatus.Id == y.TimeTableStatus.Id;
                bool CitiesFromAreEqual = x.TimeTableStatus.CityFrom.Id == y.TimeTableStatus.CityFrom.Id;
                bool CitiesToAreEqual = x.TimeTableStatus.CityTo.Id == y.TimeTableStatus.CityTo.Id;
                bool FlightWebsitesAreEqual = x.TimeTableStatus.FlightWebsite.Id == y.TimeTableStatus.FlightWebsite.Id;

                return IdsAreEqual && CitiesFromAreEqual && CitiesToAreEqual && FlightWebsitesAreEqual;
            }

            public int GetHashCode(TimeTableStatusHelper obj)
            {
                return obj.GetHashCode();
            }
        }
    }

    
}
