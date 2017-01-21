using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Command
{
    public interface IFlightsCommand
    {
        void Add(FlightsDto.Flight flight);

        void DeleteFlightsBySearchDate(DateTime date);

        void AddRange(IEnumerable<FlightsDto.Flight> flights);

        void DeleteFlightsBySearchCriteria(FlightsDto.SearchCriteria searchCriteria);
    }
}
