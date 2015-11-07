using System;
using System.Collections.Generic;
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

        List<FlightsDto.Flight> Convert(List<FlightsDomain.Flights> flight);
    }
}
