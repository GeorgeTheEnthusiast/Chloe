using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface IFlightsConverter
    {
        FlightsDomain.Flights Convert(FlightsDto.Flight flight);

        IEnumerable<FlightsDto.Flight> Convert(IEnumerable<FlightsDomain.Flights> flight);

        IEnumerable<FlightsDomain.Flights> Convert(IEnumerable<FlightsDto.Flight> flight);

        IEnumerable<FlightsDto.Flight> Convert(DbSet<FlightsDomain.Flights> flights);
    }
}
