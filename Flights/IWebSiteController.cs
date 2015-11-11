using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;

namespace Flights
{
    public interface IWebSiteController
    {
        List<Flight> GetFlights(SearchCriteria searchCriteria);
    }
}
