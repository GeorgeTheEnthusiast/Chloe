using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;
using OpenQA.Selenium;

namespace Flights
{
    public interface IWizzAirWebSiteController
    {
        void NavigateToUrl();

        void FillCityFrom(SearchCriteria searchCriteria);

        void FillCityTo(SearchCriteria searchCriteria);

        void FillDate(SearchCriteria searchCriteria);

        void FindFlights();

        List<Flight> GetFlights(SearchCriteria searchCriteria);
    }
}
