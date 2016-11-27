using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Chloe.Converters;
using Chloe.Domain.Command;
using Chloe.Domain.Query;
using Chloe.Dto;
using Chloe.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Chloe.Dto.Enums.FlightWebsite;

namespace Chloe.Controllers.TimeTableControllers
{
    public class KrakowAirportTimeTableController : ITimeTableController
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
        private Chloe.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;
        private City _airportCity;

        public KrakowAirportTimeTableController(IWebDriver driver,
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
            _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.KrakowAirport);
            _airportCity = _cityQuery.GetCityByName("Kraków");
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

            GoToTimeTableSite();

            ExpandAllTimeTables();

            Thread.Sleep(TimeSpan.FromSeconds(3));
            
            CreateArrivals();

            Thread.Sleep(TimeSpan.FromSeconds(3));

            CreateDepartures();
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
            var tab = _webDriverWait.Until(x => x.FindElement(By.ClassName("tab2")));

            tab.Click();
        }

        private void CreateDepartures()
        {
            SelectDepartures();

            var table = _webDriverWait.Until(x => x.FindElements(By.CssSelector("table[class='default-table']"))[1]);
            var trElements = table.FindElements(By.XPath("tbody/tr"));
            City cityTo = new City();
            int i = 0;
            var timeTableStatuses = _timeTableStatusQuery.GetTimeTableStatusesByWebSiteId(_flightWebsite.Id);

            foreach (var tr in trElements)
            {
                try
                {
                    var cityToElement = tr.FindElement(By.TagName("div"));
                    cityTo = GetCity(cityToElement);
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

                var tableStatuses = timeTableStatuses as IList<TimeTableStatus> ?? timeTableStatuses.ToList();
                try
                {
                    var daysInWeekTable = tr.FindElement(By.TagName("table"));
                    List<int> daysInWeek = GetWeekDays(daysInWeekTable);

                    var departureDateElement = tr.FindElements(By.ClassName("col4"))[0];
                    TimeSpan departureTime = GetDepartureTime(departureDateElement);

                    var arrivalDateElement = tr.FindElements(By.ClassName("col4"))[1];
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

                    var carrierElement = tr.FindElement(By.ClassName("col6"));
                    Carrier carrier = GetCarrier(carrierElement);

                    var periodElement = tr.FindElement(By.ClassName("col8"));
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

                        _logger.Info("Adding new timeTable data [{0}/{1}]...", i, trElements.Count);
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

        private void CreateArrivals()
        {
            var table = _webDriverWait.Until(x => x.FindElements(By.CssSelector("table[class='default-table']"))[0]);
            var trElements = table.FindElements(By.XPath("tbody/tr"));
            City cityFrom = new City();
            int i = 0;
            var timeTableStatuses = _timeTableStatusQuery.GetTimeTableStatusesByWebSiteId(_flightWebsite.Id);

            foreach (var tr in trElements)
            {
                try
                {
                    var cityToElement = tr.FindElement(By.TagName("div"));
                    cityFrom = GetCity(cityToElement);
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

                var tableStatuses = timeTableStatuses as IList<TimeTableStatus> ?? timeTableStatuses.ToList();
                try
                {
                    var daysInWeekTable = tr.FindElement(By.TagName("table"));
                    List<int> daysInWeek = GetWeekDays(daysInWeekTable);

                    var departureDateElement = tr.FindElements(By.ClassName("col4"))[0];
                    TimeSpan departureTime = GetDepartureTime(departureDateElement);

                    var arrivalDateElement = tr.FindElements(By.ClassName("col4"))[1];
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

                    var carrierElement = tr.FindElement(By.ClassName("col6"));
                    Carrier carrier = GetCarrier(carrierElement);

                    var periodElement = tr.FindElement(By.ClassName("col8"));
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

                        _logger.Info("Adding new timeTable data [{0}/{1}]...", i, trElements.Count);
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


        private void AddData()
        {
            
        }

        private List<int> GetWeekDays(IWebElement tableWebElement)
        {
            List<int> result = new List<int>();

            var days = tableWebElement.FindElements(By.TagName("span"));

            foreach (var element in days)
            {
                var style = element.GetAttribute("style");

                switch (style)
                {
                    case "color: rgb(204, 204, 204);":
                        break;
                    case "font-weight: bold; color: rgb(12, 51, 108);":
                        int dayNumber = int.Parse(element.GetAttribute("innerHTML"));
                        result.Add(dayNumber);
                        break;
                    default:
                        throw new NotSupportedException(string.Format("Style [{0}] is not supported!", style));
                }
            }

            return result;
        }

        private TimeSpan GetDepartureTime(IWebElement webElement)
        {
            var time = webElement
                .GetAttribute("innerHTML");
            time = time.Replace("<strong>", "")
                .Replace("</strong>", "")
                .Trim();

            TimeSpan result = TimeSpan.Parse(time);

            return result;
        }

        private TimeSpan GetArrivalTime(IWebElement webElement)
        {
            var time = webElement
                .GetAttribute("innerHTML");
            time = time.Replace("<strong>", "")
                .Replace("</strong>", "")
                .Trim();

            TimeSpan result = TimeSpan.Parse(time);

            return result;
        }

        private Carrier GetCarrier(IWebElement webElement)
        {
            Carrier result = _carrierCommand.Merge(webElement.Text);

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
