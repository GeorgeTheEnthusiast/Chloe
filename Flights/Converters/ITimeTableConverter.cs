using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ITimeTableConverter
    {
        FlightsDomain.TimeTable Convert(FlightsDto.TimeTable input);

        FlightsDto.TimeTable Convert(FlightsDomain.TimeTable input);
    }
}
