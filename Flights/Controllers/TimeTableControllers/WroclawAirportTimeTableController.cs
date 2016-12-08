using System;
using System.Collections.Generic;
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
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Flights.Dto.Enums.FlightWebsite;

namespace Flights.Controllers.TimeTableControllers
{
    public class WroclawAirportTimeTableController : ITimeTableController
    {
        private readonly ITimeTableCommand _timeTableCommand;
        private readonly ICitiesCommand _citiesCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ITimeTablePeriodConverter _timeTablePeriodConverter;
        private readonly ICityQuery _cityQuery;
        private readonly ICarrierCommand _carrierCommand;
        private readonly ITimeTableStatusCommand _timeTableStatusCommand;
        private readonly ITimeTableStatusQuery _timeTableStatusQuery;
        private readonly IWebDriver _driver;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;
        private City _airportCity;

        public WroclawAirportTimeTableController(IWebDriver driver,
            ITimeTableCommand timeTableCommand,
            ICitiesCommand citiesCommand,
            IFlightWebsiteQuery flightWebsiteQuery,
            ITimeTablePeriodConverter timeTablePeriodConverter,
            ICityQuery cityQuery,
            ICarrierCommand carrierCommand,
            ITimeTableStatusCommand timeTableStatusCommand,
            ITimeTableStatusQuery timeTableStatusQuery
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (timeTableCommand == null) throw new ArgumentNullException("timeTableCommand");
            if (citiesCommand == null) throw new ArgumentNullException("citiesCommand");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (timeTablePeriodConverter == null) throw new ArgumentNullException("timeTablePeriodConverter");
            if (cityQuery == null) throw new ArgumentNullException("cityQuery");
            if (carrierCommand == null) throw new ArgumentNullException("carrierCommand");
            if (timeTableStatusCommand == null) throw new ArgumentNullException("timeTableStatusCommand");
            if (timeTableStatusQuery == null) throw new ArgumentNullException("timeTableStatusQuery");

            _driver = driver;
            _timeTableCommand = timeTableCommand;
            _citiesCommand = citiesCommand;
            _flightWebsiteQuery = flightWebsiteQuery;
            _timeTablePeriodConverter = timeTablePeriodConverter;
            _cityQuery = cityQuery;
            _carrierCommand = carrierCommand;
            _timeTableStatusCommand = timeTableStatusCommand;
            _timeTableStatusQuery = timeTableStatusQuery;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.WroclawAirport);
            _airportCity = _cityQuery.GetCityByName("Wrocław");
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
            NavigateToUrl();

            var cities = GetAllCities();
            
            foreach (var city in cities.Keys)
            {
                var dropDownList = _driver.FindElement(By.Name("lotnisko"));
                var selectElement = new SelectElement(dropDownList);
                selectElement.SelectByText(city);

                CreateDepartures(cities[city]);
                CreateArrivals(cities[city]);
            }
        }

        private Dictionary<string, City> GetAllCities()
        {
            var dropDownList = _driver.FindElement(By.Name("lotnisko"));
            var selectElement = new SelectElement(dropDownList);
            Dictionary<string, City> result = new Dictionary<string, City>();

            foreach (var option in selectElement.Options)
            {
                if (option.Text == "-- wybierz lotnisko --")
                    continue;

                int indexOfOpeningBracket = option.Text.IndexOf('[');

                City city = new City();
                city.Name = option.Text.Substring(0, indexOfOpeningBracket).Trim();
                city.Alias = option.Text.Substring(indexOfOpeningBracket + 1, 3);

                if (city.Name.Contains("*"))
                    continue;

                city = _citiesCommand.Merge(city);

                result[option.Text] = city;
            }

            return result;
        } 

        private void GoToTimeTableSite()
        {
            var passengerInfos = _webDriverWait.Until(x => x.FindElement(By.PartialLinkText("lotach")));
            passengerInfos.Click();

            var timeTableScheduler = _webDriverWait.Until(x => x.FindElement(By.PartialLinkText("regularnych")));
            timeTableScheduler.Click();
        }

        private void ExpandAllTimeTables()
        {
            var select = _webDriverWait.Until(x => x.FindElement(By.ClassName("sbSelector")));
            select.Click();
            var allAirports = _webDriverWait.Until(x => x.FindElement(By.CssSelector("li[class='sbSelected']")));
            allAirports.Click();
        }

        private void SelectDepartures()
        {
            var tab = _webDriverWait.Until(x => x.FindElement(By.Id("odloty")));

            tab.Click();
        }

        private void SelectArrivals()
        {
            var tab = _webDriverWait.Until(x => x.FindElement(By.Id("przyloty")));

            tab.Click();
        }

        private void CreateDepartures(City cityTo)
        {
            SelectDepartures();
            
            var table = _webDriverWait.Until(x => x.FindElement(By.Id("table-odloty")));
            var trElements = table.FindElements(By.XPath("tbody[2]/tr"));
            int i = 0;
            var timeTableStatuses = _timeTableStatusQuery.GetTimeTableStatusesByWebSiteId(_flightWebsite.Id);

            foreach (var tr in trElements)
            {
                var tds = tr.FindElements(By.TagName("td"));
                
                var tableStatuses = timeTableStatuses as IList<TimeTableStatus> ?? timeTableStatuses.ToList();
                try
                {
                    var daysInWeekTable = tds[3];
                    List<int> daysInWeek = GetWeekDays(daysInWeekTable);

                    var departureDateElement = tds[0];
                    TimeSpan departureTime = GetDepartureTime(departureDateElement);

                    var arrivalDateElement = tds[1];
                    TimeSpan arrivalTime = GetArrivalTime(arrivalDateElement);

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

                    var carrierElement = tds[6];
                    Carrier carrier = GetCarrier(carrierElement);

                    var periodElement = tds[5];
                    DateTime dateFrom;
                    DateTime dateTo;
                    GetTimeTablePeriod(periodElement, out dateFrom, out dateTo);

                    IEnumerable<DateTime> timeTableDates = _timeTablePeriodConverter.Convert(daysInWeek, dateFrom,
                        dateTo);

                    foreach (var date in timeTableDates)
                    {
                        TimeTableStatus timeTableStatus = new TimeTableStatus()
                        {
                            FlightWebsite = _flightWebsite,
                            CityFrom = _airportCity,
                            CityTo = cityTo,
                            SearchDate = DateTime.Now
                        };

                        if (tableStatuses.Any(x => x.CityFrom.Id == _airportCity.Id
                                                       && x.CityTo.Id == cityTo.Id
                                                       && x.FlightWebsite.Id == _flightWebsite.Id
                                                       && x.SearchDate != null
                                                       &&
                                                       DateTime.Compare(x.SearchDate.Value.Date,
                                                           DateTime.Now.AddMonths(-3).Date) == 1))
                        {
                            continue;
                        }

                        TimeTable timeTable = new TimeTable()
                        {
                            FlightWebsite = _flightWebsite,
                            Carrier = carrier,
                            CityFrom = _airportCity,
                            CityTo = cityTo,
                            DepartureDate = date.AddMinutes(departureTime.TotalMinutes)
                        };
                        timeTable.ArrivalDate = timeTable.DepartureDate.AddMinutes(timeDifference.TotalMinutes);

                        _logger.Info("Adding new timeTable data [[{0}] -- [{1}]] --> [{2}/{3}]...", timeTable.CityFrom.Name, timeTable.CityTo.Name, i, trElements.Count);
                        _timeTableStatusCommand.Merge(timeTableStatus);
                        _timeTableCommand.Merge(timeTable);
                    }
                }
                catch (NoSuchElementException)
                {
                    
                }
                catch (Exception ex)
                {
                    _logger.Error("Error reading tr element:");
                    _logger.Error(tr.GetAttribute("innerHTML"));
                    _logger.Error(ex);
                }

                i++;
            }
        }

        private void CreateArrivals(City cityFrom)
        {
            SelectArrivals();

            var table = _webDriverWait.Until(x => x.FindElement(By.Id("table-przyloty")));
            var trElements = table.FindElements(By.XPath("tbody[2]/tr"));
            int i = 0;
            var timeTableStatuses = _timeTableStatusQuery.GetTimeTableStatusesByWebSiteId(_flightWebsite.Id);

            foreach (var tr in trElements)
            {
                var tds = tr.FindElements(By.TagName("td"));

                var tableStatuses = timeTableStatuses as IList<TimeTableStatus> ?? timeTableStatuses.ToList();
                try
                {
                    var daysInWeekTable = tds[3];
                    List<int> daysInWeek = GetWeekDays(daysInWeekTable);

                    var departureDateElement = tds[0];
                    TimeSpan departureTime = GetDepartureTime(departureDateElement);

                    var arrivalDateElement = tds[1];
                    TimeSpan arrivalTime = GetArrivalTime(arrivalDateElement);

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

                    var carrierElement = tds[6];
                    Carrier carrier = GetCarrier(carrierElement);

                    var periodElement = tds[5];
                    DateTime dateFrom;
                    DateTime dateTo;
                    GetTimeTablePeriod(periodElement, out dateFrom, out dateTo);

                    IEnumerable<DateTime> timeTableDates = _timeTablePeriodConverter.Convert(daysInWeek, dateFrom,
                        dateTo);

                    foreach (var date in timeTableDates)
                    {
                        TimeTableStatus timeTableStatus = new TimeTableStatus()
                        {
                            FlightWebsite = _flightWebsite,
                            CityFrom = cityFrom,
                            CityTo = _airportCity,
                            SearchDate = DateTime.Now
                        };

                        if (tableStatuses.Any(x => x.CityFrom.Id == cityFrom.Id
                                                       && x.CityTo.Id == _airportCity.Id
                                                       && x.FlightWebsite.Id == _flightWebsite.Id
                                                       && x.SearchDate != null
                                                       &&
                                                       DateTime.Compare(x.SearchDate.Value.Date,
                                                           DateTime.Now.AddMonths(-3).Date) == 1))
                        {
                            continue;
                        }

                        TimeTable timeTable = new TimeTable()
                        {
                            FlightWebsite = _flightWebsite,
                            Carrier = carrier,
                            CityFrom = cityFrom,
                            CityTo = _airportCity,
                            DepartureDate = date.AddMinutes(departureTime.TotalMinutes)
                        };
                        timeTable.ArrivalDate = timeTable.DepartureDate.AddMinutes(timeDifference.TotalMinutes);

                        _logger.Info("Adding new timeTable data [[{0}] -- [{1}]] --> [{2}/{3}]...", timeTable.CityFrom.Name, timeTable.CityTo.Name, i, trElements.Count);
                        _timeTableStatusCommand.Merge(timeTableStatus);
                        _timeTableCommand.Merge(timeTable);
                    }
                }
                catch (NoSuchElementException)
                {

                }
                catch (Exception ex)
                {
                    _logger.Error("Error reading tr element:");
                    _logger.Error(tr.GetAttribute("innerHTML"));
                    _logger.Error(ex);
                }

                i++;
            }
        }

        private List<int> GetWeekDays(IWebElement tableWebElement)
        {
            List<int> result = new List<int>();

            var days = tableWebElement.FindElements(By.TagName("div"));

            foreach (var element in days)
            {
                var text = element.Text.Trim();
                int number;

                if (!int.TryParse(text, out number))
                    continue;

                result.Add(number);
            }

            return result;
        }

        private TimeSpan GetDepartureTime(IWebElement webElement)
        {
            var time = webElement.FindElement(By.TagName("span"));

            TimeSpan result = TimeSpan.Parse(time.Text);

            return result;
        }

        private TimeSpan GetArrivalTime(IWebElement webElement)
        {
            TimeSpan result = TimeSpan.Parse(webElement.Text);

            return result;
        }

        private Carrier GetCarrier(IWebElement webElement)
        {
            var carrierWebElement = webElement.FindElement(By.TagName("a"));

            Carrier result = _carrierCommand.Merge(carrierWebElement.Text);

            return result;
        }

        private void GetTimeTablePeriod(IWebElement webElement, out DateTime dateFrom, out DateTime dateTo)
        {
            int indexOfDash = webElement.Text.IndexOf(" - ");
            string dateFromString = webElement.Text.Substring(0, indexOfDash);
            string dateToString = webElement.Text.Substring(indexOfDash + 3, 10);
            dateFrom = DateTime.Parse(dateFromString);
            dateTo = DateTime.Parse(dateToString);
        }

        private City GetCity(IWebElement webElement)
        {
            City result = new City();
            string cityName = webElement.GetAttribute("innerHTML");
            int indexOfOpeningParanthesis = cityName.IndexOf('(');

            result.Name = cityName
                .Substring(0, indexOfOpeningParanthesis)
                .Trim();
            result.Alias = cityName.Substring(indexOfOpeningParanthesis + 1, 3);

            result = _citiesCommand.Merge(result);

            return result;
        }
    }
}
