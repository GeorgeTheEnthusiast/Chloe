using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface INetConverter
    {
        FlightsDomain.Net Convert(FlightsDto.Net net);

        FlightsDto.Net Convert(FlightsDomain.Net net);
    }
}
