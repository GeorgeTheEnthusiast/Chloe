using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;

namespace Flights
{
    public interface IFlightService
    {
        List<Flight> GetFlights(SearchCriteria searchCriteria);
    }
}
