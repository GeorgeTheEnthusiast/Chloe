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
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Chloe.Dto.Enums.FlightWebsite;

namespace Chloe.Controllers.FlightsControllers
{
    public class RyanAirWebSiteController : IWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly IRyanAirDateConverter _ryanAirDateConverter;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierCommand _carrierCommand;
        private Chloe.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string FlightIsNotAvailableOnThisDay = "W tym dniu nie ma żadnych lotów";
        private readonly string FlightIsAvailable = "OK";
        //private readonly string FlightDepartureDateIsRequired = "Wymagana jest data wylotu.";
        private WebDriverWait _webDriverWait;

        public RyanAirWebSiteController(IWebDriver driver, 
            ICurrienciesCommand currienciesCommand, 
            IRyanAirDateConverter ryanAirDateConverter,
            IFlightWebsiteQuery flightWebsiteQuery,
            ICarrierCommand carrierCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (ryanAirDateConverter == null) throw new ArgumentNullException("ryanAirDateConverter");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (carrierCommand == null) throw new ArgumentNullException("carrierCommand");

            _driver = driver;
            _currienciesCommand = currienciesCommand;
            _ryanAirDateConverter = ryanAirDateConverter;
            _flightWebsiteQuery = flightWebsiteQuery;
            _carrierCommand = carrierCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(20));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }

        private void MakeTicketOneWay()
        {
            IReadOnlyCollection<IWebElement> typeOfTicketWebElements = _driver.FindElements(By.ClassName("flight-search-type-option"));
            IWebElement oneWayTicketWebElement = typeOfTicketWebElements.First(x => x.Text == "W jedną stronę");
            oneWayTicketWebElement.Click();
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-from']"));
            

            Actions actions = new Actions(_driver);
            actions.MoveToElement(fromCityWebElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.SendKeys(cityName);
            actions.SendKeys(Keys.Tab);
            actions.Build().Perform();

            if (IsInputWasFilledCorrectly(cityName, fromCityWebElement) == false)
            {
                throw new InputWasNotFilledCorrectlyException()
                {
                    Name = "CityFrom"
                };
            }
        }

        private void FillCityTo(string cityName)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-to']"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(toCityWebElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.SendKeys(cityName);
            actions.SendKeys(Keys.Enter);
            actions.Build().Perform();

            if (IsInputWasFilledCorrectly(cityName, toCityWebElement) == false)
            {
                throw new InputWasNotFilledCorrectlyException()
                {
                    Name = "CityTo"
                };
            }
        }

        private void FillDate(DateTime departureDate)
        {
            IWebElement dayPickerWebElement = _driver.FindElement(By.Name("dateInput0"));
            IWebElement monthPickerWebElement = _driver.FindElement(By.Name("dateInput1"));
            IWebElement yearPickerWebElement = _driver.FindElement(By.Name("dateInput2"));
            
            SetDatePickerDigital(dayPickerWebElement, departureDate.Day.ToString("00"));
            SetDatePickerDigital(monthPickerWebElement, departureDate.Month.ToString("00"));
            SetDatePickerDigital(yearPickerWebElement, departureDate.Year.ToString("0000"));
        }

        private bool IsDateWasTypedCorrectly(DateTime departureDate)
        {
            string day = _driver.FindElement(By.Name("dateInput0"))
                .GetAttribute("value");
            string month = _driver.FindElement(By.Name("dateInput1"))
                .GetAttribute("value");
            string year = _driver.FindElement(By.Name("dateInput2"))
                .GetAttribute("value");
            
            return (day == departureDate.Day.ToString("00")
                && month == departureDate.Month.ToString("00")
                && year == departureDate.Year.ToString("0000"));
        }

        private void SetDatePickerDigital(IWebElement webElement, string value)
        {
            if (webElement == null) throw new ArgumentNullException("webElement");
            if (value == null) throw new ArgumentNullException("value");

            const int maxDigitals = 4;

            Actions actions = new Actions(_driver);
            actions.MoveToElement(webElement);
            actions.Click();

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.ArrowRight);

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.Backspace);

            actions.Build().Perform();

            actions.SendKeys(value);
            actions.Build().Perform();
        }

        private void ProceedToDatesForm()
        {
            GoToChloePage();
        }

        private void GoToChloePage()
        {
            var buttonWebElements = _driver.FindElements(By.CssSelector("button[class='core-btn-primary core-btn-block core-btn-big']"));
            foreach (var btn in buttonWebElements)
            {
                if (btn.Displayed)
                {
                    Actions actions = new Actions(_driver);
                    actions.MoveToElement(btn);
                    actions.Click();
                    actions.Build().Perform();
                }
            }
        }

        private string GetInputValidationState()
        {
            IWebElement errorWebElement = null;
            
            try
            {
                errorWebElement = _driver.FindElement(By.ClassName("ryanair-error-tooltip"));

                if (!errorWebElement.Displayed)
                    return FlightIsAvailable;
            }
            catch
            {
                return FlightIsAvailable;
            }

            return errorWebElement.FindElement(By.TagName("span")).Text;
        }

        public List<Flight> GetChloe(SearchCriteria searchCriteria)
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.RyanAir);

            List<Flight> result = new List<Flight>();
            
            //return result;

            if (searchCriteria.FlightWebsite.Id != _flightWebsite.Id)
                return result;

            NavigateToUrl();

            MakeTicketOneWay();

            FillCityFrom(searchCriteria.CityFrom.Name);

            FillCityTo(searchCriteria.CityTo.Name);

            ProceedToDatesForm();
            
            DateTime departureDate = searchCriteria.DepartureDate.Date;
            int daysToAdd = 1;

            while (true)
            {
                FillDate(departureDate);
                
                if (IsDateWasTypedCorrectly(departureDate) == false)
                {
                    _logger.Info("Dates are not equal, trying again...");
                    continue;
                }

                string errorText = GetInputValidationState();

                if (errorText == FlightIsNotAvailableOnThisDay)
                    departureDate = departureDate.AddDays(daysToAdd);
                else if (errorText == FlightIsAvailable)
                    break;

                if (DateTime.Compare(departureDate.Date, searchCriteria.DepartureDate.AddDays(2).Date) == 0)
                    daysToAdd = -1;
                else if (DateTime.Compare(departureDate.Date, searchCriteria.DepartureDate.AddDays(-2).Date) == 0)
                    return result;
            }

            GoToChloePage();
            
            if (IsNextPageLoadedSuccessfully() == false)
            {
                throw new FlightsPageIsNotLoadedCorrectlyException();
            }
            
            result = CollectChloe(searchCriteria);
            
            return result;
        } 

        private bool IsNextPageLoadedSuccessfully()
        {
            try
            {
                _webDriverWait.Until(x => x.FindElements(By.CssSelector("span[class='preamble']")).Any(y => y.Text == "Wybierz lot na trasie wylotu"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool IsInputWasFilledCorrectly(string text, IWebElement webElement)
        {
            var value = webElement.GetAttribute("value");

            if (string.IsNullOrEmpty(value))
                return false;

            return webElement.GetAttribute("value") == text;
        }

        private Flight GetOneItemFromCarousel(IWebElement webElement, SearchCriteria searchCriteria, DateTime departureDate)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            Flight result = new Flight()
            {
                SearchDate = DateTime.Now,
                SearchCriteria = searchCriteria,
                IsDirect = true
            };
            
            string dateLong =  webElement.FindElement(By.ClassName("date")).GetAttribute("innerHTML");
            result.DepartureTime = _ryanAirDateConverter.Convert(departureDate, dateLong);
            
            var className = webElement
                .FindElement(By.ClassName("carousel-item"))
                .GetAttribute("class");

            if (className.Contains("item-not-available"))
                return null;
                
            string price = webElement.FindElement(By.ClassName("fare")).Text;

            if (price.Contains("∞"))
                return null;

            if (string.IsNullOrEmpty(price))
            {
                _logger.Info("Price is empty for departure date [{0}]", result.DepartureTime.Date.ToShortDateString());
                throw new PriceIsEmptyException();
            }

            result.DepartureTime = addTimeToDepartureDate(result.DepartureTime);
            AddCurrency(ref result, price);
            result.Carrier = _carrierCommand.Merge("RyanAir");

            return result;
        }

        private DateTime GetDateFromCarousel(IWebElement webElement, DateTime searchDepartureDate)
        {
            string dateLong = webElement.FindElement(By.ClassName("date")).GetAttribute("innerHTML");

            DateTime result = _ryanAirDateConverter.Convert(searchDepartureDate, dateLong);

            return result;
        }

        private void AddCurrency(ref Flight flightToAddCurrency, string price)
        {
            if (string.IsNullOrEmpty(price))
                throw new PriceIsEmptyException();

            string[] priceArray = price.Split(new [] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);

            if (priceArray.Count() == 1)
                priceArray = price.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = priceArray.Last()
            });

            string joinedPriceValue = string.Join("", priceArray.Reverse().Skip(1).Reverse())
                .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            decimal parsedPrice = 0;

            if (decimal.TryParse(joinedPriceValue, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedPrice))
            {
                flightToAddCurrency.Price = parsedPrice;
            }
            else
            {
                _logger.Error("Could not parse price value: [{0}]", joinedPriceValue);
                throw new FormatException("Price is invalid!");
            }
        }

        private List<Flight> CollectChloe(SearchCriteria searchCriteria)
        {
            var slideActive = _webDriverWait.Until(
                x => x.FindElement(By.CssSelector("div[class='wrapper']"))
                .FindElements(By.ClassName("slide"))
                .ToList()
                .FirstOrDefault(y => y.GetAttribute("class").Contains("active")));
            var Chloelides = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='wrapper']")).FindElements(By.ClassName("slide")));
            
            DateTime activeSlideDateTime = GetDateFromCarousel(slideActive, searchCriteria.DepartureDate.Date);
            Dictionary<IWebElement, DateTime> slideDatesDictionary = new Dictionary<IWebElement, DateTime>();
            Dictionary<IWebElement, DateTime> slideDatesDictionaryLoop = new Dictionary<IWebElement, DateTime>();
            bool newYear = false;
            List<Flight> result = new List<Flight>();

            foreach (var slide in Chloelides)
            {
                slideDatesDictionary[slide] = GetDateFromCarousel(slide, searchCriteria.DepartureDate);
            }
            slideDatesDictionaryLoop = slideDatesDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);

            newYear = slideDatesDictionary.Any(x => x.Value.Month == 12)
                    && slideDatesDictionary.Any(x => x.Value.Month == 1);

            if (newYear)
            {
                foreach (var slide in slideDatesDictionaryLoop)
                {
                    if (searchCriteria.DepartureDate.Month == 12)
                    {
                        if (slide.Value.Month == 1)
                        {
                            slideDatesDictionary[slide.Key] = slide.Value.AddYears(1);
                        }
                    }
                    else
                    {
                        if (slide.Value.Month == 12)
                        {
                            slideDatesDictionary[slide.Key] = slide.Value.AddYears(-1);
                        }
                    }
                }

                if (searchCriteria.DepartureDate.Month == 12)
                {
                    if (activeSlideDateTime.Month == 1)
                    {
                        activeSlideDateTime = activeSlideDateTime.AddYears(1);
                    }
                }
                else
                {
                    if (activeSlideDateTime.Month == 12)
                    {
                        activeSlideDateTime = activeSlideDateTime.AddYears(-1);
                    }
                }
            }

            slideDatesDictionary =
                slideDatesDictionary.Where(x => DateTime.Compare(x.Value.Date, searchCriteria.DepartureDate.AddDays(2).Date) <= 0
                && DateTime.Compare(x.Value.Date, searchCriteria.DepartureDate.AddDays(-2).Date) >= 0)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            
            if (DateTime.Compare(searchCriteria.DepartureDate.AddMonths(-1).Date, activeSlideDateTime.Date) == 1
                || DateTime.Compare(searchCriteria.DepartureDate.AddMonths(1).Date, activeSlideDateTime.Date) == -1)
                throw new SearchDepartureDateIsIncorrectException();


            var firstSlide = slideDatesDictionary
                .FirstOrDefault(x => DateTime.Compare(x.Value.Date, searchCriteria.DepartureDate.AddDays(-2).Date) == 0);
            var lastSlide = slideDatesDictionary
                .FirstOrDefault(x => DateTime.Compare(x.Value.Date, searchCriteria.DepartureDate.AddDays(2).Date) == 0);
            var middleSlide = slideDatesDictionary
                .FirstOrDefault(x => DateTime.Compare(x.Value.Date, searchCriteria.DepartureDate.Date) == 0);
            var activeSlide = new KeyValuePair<IWebElement, DateTime>();
            bool moveRight;

            if (firstSlide.Key.Displayed == true)
            {
                firstSlide.Key.Click();
                activeSlide = firstSlide;
                moveRight = true;
            }
            else
            {
                lastSlide.Key.Click();
                activeSlide = lastSlide;
                moveRight = false;
            }

            var flight = new Flight();

            for (int i = 0; i < 5; i++)
            {
                if (i != 0)
                {
                    if (moveRight)
                        activeSlide = MoveCarouselRight(slideDatesDictionary, activeSlide.Key, activeSlide.Value);
                    else
                        activeSlide = MoveCarouselLeft(slideDatesDictionary, activeSlide.Key, activeSlide.Value);
                }

                flight = GetOneItemFromCarousel(activeSlide.Key, searchCriteria, activeSlide.Value);

                if (flight != null)
                    result.Add(flight);
            }

            middleSlide.Key.Click();

            return result;
        }

        private KeyValuePair<IWebElement, DateTime> MoveCarouselLeft(Dictionary<IWebElement, DateTime> slideDates, IWebElement activeSlide, DateTime activeSlideDate)
        {
            var previousSlide = slideDates
                .FirstOrDefault(x => DateTime.Compare(x.Value.Date, activeSlideDate.AddDays(-1).Date) == 0);

            previousSlide.Key.Click();
            return previousSlide;
        }

        private KeyValuePair<IWebElement, DateTime> MoveCarouselRight(Dictionary<IWebElement, DateTime> slideDates, IWebElement activeSlide, DateTime activeSlideDate)
        {
            var nextSlide = slideDates
                .FirstOrDefault(x => DateTime.Compare(x.Value.Date, activeSlideDate.AddDays(1).Date) == 0);

            nextSlide.Key.Click();
            return nextSlide;
        }

        private DateTime addTimeToDepartureDate(DateTime departureDate)
        {
            var departureTimeHolderElement =
                _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='flight-holder']")));
            var departureTimeElement = departureTimeHolderElement
                .FindElement(By.ClassName("starting-point"))
                .FindElement(By.ClassName("time"));
            TimeSpan time = TimeSpan.Parse(departureTimeElement.Text);

            return new DateTime(
                departureDate.Year,
                departureDate.Month,
                departureDate.Day,
                time.Hours,
                time.Minutes,
                0);
        }
    }
}
