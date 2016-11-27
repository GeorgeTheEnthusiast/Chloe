using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Chloe.Dto.Enums.FlightWebsite;

namespace Chloe.Controllers.TimeTableControllers
{
    public class WizzAirTimeTableController : ITimeTableController
    {
        private readonly ITimeTableCommand _timeTableCommand;
        private readonly ICitiesCommand _citiesCommand;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ITimeTablePeriodConverter _timeTablePeriodConverter;
        private readonly ICityQuery _cityQuery;
        private readonly ICarrierCommand _carrierCommand;
        private readonly ICarrierQuery _carrierQuery;
        private readonly ITimeTableStatusCommand _timeTableStatusCommand;
        private readonly IWebDriver _driver;
        private Chloe.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private WebDriverWait _webDriverWait;
        private Carrier _carrier;

        public WizzAirTimeTableController(IWebDriver driver,
            ITimeTableCommand timeTableCommand,
            ICitiesCommand citiesCommand,
            IFlightWebsiteQuery flightWebsiteQuery,
            ITimeTablePeriodConverter timeTablePeriodConverter,
            ICityQuery cityQuery,
            ICarrierCommand carrierCommand,
            ICarrierQuery carrierQuery,
            ITimeTableStatusCommand timeTableStatusCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (timeTableCommand == null) throw new ArgumentNullException("timeTableCommand");
            if (citiesCommand == null) throw new ArgumentNullException("citiesCommand");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (timeTablePeriodConverter == null) throw new ArgumentNullException("timeTablePeriodConverter");
            if (cityQuery == null) throw new ArgumentNullException("cityQuery");
            if (carrierCommand == null) throw new ArgumentNullException("carrierCommand");
            if (carrierQuery == null) throw new ArgumentNullException("carrierQuery");
            if (timeTableStatusCommand == null) throw new ArgumentNullException("timeTableStatusCommand");

            _driver = driver;
            _timeTableCommand = timeTableCommand;
            _citiesCommand = citiesCommand;
            _flightWebsiteQuery = flightWebsiteQuery;
            _timeTablePeriodConverter = timeTablePeriodConverter;
            _cityQuery = cityQuery;
            _carrierCommand = carrierCommand;
            _carrierQuery = carrierQuery;
            _timeTableStatusCommand = timeTableStatusCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.WizzAirAirport);
            _carrier = _carrierQuery.GetCarrierByName("WizzAir");
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

            ExpandAllCities();

            List<City> cities = GetAllCities();
            List<City> citiesToRepeat = new List<City>();
			List<TimeTableStatus> timeTableStatusList = new List<TimeTableStatus>();
			
            Stopwatch counter = new Stopwatch();
            counter.Start();
            
            foreach (var city in cities)
            {
                FillCityFrom(city.Name);
                List<City> citiesTo = GetAllCitiesTo();

                foreach (var cityTo in citiesTo)
                {
                    var timeTableStatus = new TimeTableStatus()
                    {
                        CityFrom = city,
                        CityTo = cityTo,
                        FlightWebsite = _flightWebsite,
                        SearchDate = null
                    };
                    var mergeResult = _timeTableStatusCommand.Merge(timeTableStatus);
					timeTableStatusList.Add(mergeResult);
                }
            }

            timeTableStatusList =
                timeTableStatusList.Where(
                    x =>
                        x.SearchDate == null ||
                        DateTime.Compare(x.SearchDate.Value.Date, DateTime.Now.AddMonths(-3).Date) == -1)
                    .ToList();

            while (cities.Count > 0)
            {
                foreach (var city in cities)
                {
                    try
                    {
                        if (counter.Elapsed.TotalMinutes >= 18)
                        {
                            _logger.Debug("Resetting the counter, restarting the website...");
                            counter.Restart();
                            NavigateToUrl();
                        }

                        WaitUntilProgressIsClosed();
                        
                        var citiesTo = (from t in timeTableStatusList
                            where t.CityFrom.Id == city.Id
                            select t.CityTo)
                            .ToList();

                        foreach (var cityTo in citiesTo)
                        {
                            var timeTableStatus = new TimeTableStatus()
                            {
                                CityFrom = city,
                                CityTo = cityTo,
                                FlightWebsite = _flightWebsite,
                                SearchDate = DateTime.Now
                            };
							
                            try
                            {
                                FillCityFrom(city.Name);
                                FillCityTo(cityTo.Name);
                                ShowTimeTable();
                                WaitUntilProgressIsClosed();
                                Create(city, cityTo);

                                _logger.Debug("Adding timeTable from [{0}] to [{1}] completed without errors.", city.Name, cityTo.Name);
                                timeTableStatusList.RemoveAll(x => x.CityFrom.Id == city.Id && x.CityTo.Id == cityTo.Id);
                                _timeTableStatusCommand.Merge(timeTableStatus);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("I have to repeat timeTable from [{0}] to [{1}]", city.Name, cityTo.Name);
                                _logger.Error(ex);

                                timeTableStatusList.Add(timeTableStatus);
                            }
                        }

                        if (timeTableStatusList.Any(x => x.CityFrom.Id == city.Id) == true)
                        {
                            citiesToRepeat.Add(city);
                        }
                        else
                        {
                            citiesToRepeat.RemoveAll(x => x.Id == city.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("I have to repeat timeTable from city [{0}]", city.Name);
                        _logger.Error(ex);

                        citiesToRepeat.Add(city);
                    }
                }

                cities = citiesToRepeat.ToList();
                _logger.Info("TimeTables left: cities: [{0}], pairs: [{1}]", cities.Count, timeTableStatusList.Count);
            }

            counter.Stop();
        }

        private void Create(City cityFrom, City cityTo)
        {
            _logger.Debug("Adding timeTable from [{0}] to [{1}]", cityFrom.Name, cityTo.Name);
            bool goRight = true;

            while (goRight)
            {
                WaitUntilProgressIsClosed();
                var timetableSliderWebElement = _webDriverWait.Until(x => x.FindElement(By.Id("timetableSlider")));
                var elements = timetableSliderWebElement
                    .FindElements(By.TagName("table"))
                    .First(x => x.GetCssValue("display") == "block" 
                            || x.GetCssValue("display") == "table")
                    .FindElements(By.CssSelector("span[class='Chloe_daylist']"));

                foreach (var element in elements)
                {
                    bool isAvailable = IsFlightIsAvailable(element);

                    if (isAvailable)
                    {
                        var dateWebElement = element.FindElement(By.TagName("strong"));
                        
                        var timeWebElements = element.FindElements(By.CssSelector("span[class='item']"));

                        foreach (var item in timeWebElements)
                        {
                            DateTime departureDate = DateTime.Parse(dateWebElement.GetAttribute("data-datetime"));

                            if (DateTime.Compare(departureDate.Date, DateTime.Now.AddYears(1)) >= 0)
                                continue;

                            TimeSpan departureTime;
                            TimeSpan arrivalTime;

                            GetDepartureAndArrivalTime(item.GetAttribute("data-time"), out departureTime, out arrivalTime);
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

                goRight = GoRight();
                WaitUntilProgressIsClosed();
            }
        }
        
        private void ScrollPageDown()
        {
            try
            {
                _driver.ExecuteJavaScript<IWebElement>("scroll(0, 2000)");
            }
            catch
            {
            }
        }

        private bool GoRight()
        {
            try
            {
                var rightWebElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("a[class='right next']")));

                WaitUntilProgressIsClosed();

                if (rightWebElement.Displayed)
                {
                    rightWebElement.Click();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                var rightWebElement = _webDriverWait.Until(x => x.FindElement(By.CssSelector("a[class='right next']")));
                
                WaitUntilProgressIsClosed();
                rightWebElement.Click();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in getting the right button!");
                throw ex;
            }
        }

        private void ExpandAllCities()
        {
            IWebElement webElement = _driver.FindElement(By.Id("WizzTimeTableControl_AutocompleteTxtDeparture"));
            webElement.Click();
        }

        private List<City> GetAllCities()
        {
            List<City> result = new List<City>();
            IWebElement webElement = _driver.FindElements(By.CssSelector("div[class='box-autocomplete inContent']"))[0];

            var citiesWebElements =
                webElement.FindElements(By.TagName("li"));

            foreach (var cityWebElement in citiesWebElements)
            {
                City c = new City();
                c.Name = cityWebElement.GetAttribute("innerHTML")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "");

                c.Alias = c.Name.Substring(c.Name.IndexOf('(') + 1, 3);

                c.Name = c.Name.Substring(0, c.Name.IndexOf('('))
                    .Trim();

                c = _citiesCommand.Merge(c);

                result.Add(c);
            }

            return result;
        }

        private List<City> GetAllCitiesTo()
        {
            List<City> result = new List<City>();
            IWebElement webElement = _webDriverWait.Until(x => x.FindElements(By.CssSelector("div[class='box-autocomplete inContent']"))[1]);

            var citiesWebElements =
                webElement.FindElements(By.TagName("li"));

            foreach (var cityWebElement in citiesWebElements)
            {
                City c = new City();
                c.Name = cityWebElement.GetAttribute("innerHTML")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "");

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
            IWebElement fromCityWebElement = _driver.FindElement(By.Id("WizzTimeTableControl_AutocompleteTxtDeparture"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(Keys.Backspace);
            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Tab);
        }

        private void FillCityTo(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.Id("WizzTimeTableControl_AutocompleteTxtArrival"));

            fromCityWebElement.Click();
            fromCityWebElement.SendKeys(Keys.Backspace);
            fromCityWebElement.SendKeys(cityName);
            fromCityWebElement.SendKeys(Keys.Tab);
        }

        private void ShowTimeTable()
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.Id("WizzTimeTableControl_ButtonSubmit"));

            fromCityWebElement.Click();
        }

        private void WaitUntilProgressIsClosed()
        {
            var progressWebElement = _driver.FindElement(By.Id("timetableOverlay"));
            IWait<IWebElement> progressWait = new DefaultWait<IWebElement>(progressWebElement);
            progressWait.Timeout = TimeSpan.FromSeconds(20);

            progressWait.Until(x => x.GetCssValue("display") == "none");
        }

        private void WaitUntilProgressIsVisible()
        {
            var progressWebElement = _driver.FindElement(By.Id("timetableOverlay"));
            IWait<IWebElement> progressWait = new DefaultWait<IWebElement>(progressWebElement);
            progressWait.Timeout = TimeSpan.FromSeconds(10);

            progressWait.Until(x => x.GetCssValue("display") == "block");
        }

        private bool IsFlightIsAvailable(IWebElement webElement)
        {
            try
            {
                var elements = webElement.FindElements(By.TagName("span"));
                
                if (elements.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception("Error in getting the flight status!", ex);
            }
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

        private void GetDepartureAndArrivalTime(string text, out TimeSpan departureTime, out TimeSpan arrivalTime)
        {
            int index = text.IndexOf('|');
            string time1 = text.Substring(0, index);
            string time2 = text.Substring(index + 1, 5);

            departureTime = TimeSpan.Parse(time1);
            arrivalTime = TimeSpan.Parse(time2);
        }
    }
}
