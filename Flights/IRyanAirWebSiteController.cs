using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;
using OpenQA.Selenium;

namespace Flights
{
    public interface IRyanAirWebSiteController
    {
        void NavigateToUrl();

        void MakeTicketOneWay();

        void FillCityFrom(SearchCriteria searchCriteria);

        void FillCityTo(SearchCriteria searchCriteria);

        void FillDate(SearchCriteria searchCriteria);

        void FindFlights();

        string GetInputValidationState();

        void SetDatePickerDigital(IWebElement webElement, string value);

        void TerminateSite();

        bool IsNextPageLoadedSuccessfully();

        bool IsInputWasFilledCorrectly(string text, IWebElement webElement);

        List<Flight> GetFlights(SearchCriteria searchCriteria);
    }
}
