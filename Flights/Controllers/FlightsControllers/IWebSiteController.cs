using System.Collections.Generic;
using Flights.Dto;

namespace Flights.Controllers.FlightsControllers
{
    public interface IWebSiteController
    {
        List<Flight> GetFlights(SearchCriteria searchCriteria);
    }
}
