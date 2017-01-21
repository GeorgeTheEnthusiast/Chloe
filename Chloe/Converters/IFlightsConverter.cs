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
        FlightsDomain.Flights Convert(FlightsDto.Flight input);

        IEnumerable<FlightsDto.Flight> Convert(IEnumerable<FlightsDomain.Flights> input);

        IEnumerable<FlightsDomain.Flights> Convert(IEnumerable<FlightsDto.Flight> input);

        IEnumerable<FlightsDto.Flight> Convert(DbSet<FlightsDomain.Flights> input);
    }
}
