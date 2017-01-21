using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ICityConverter
    {
        FlightsDomain.Cities Convert(FlightsDto.City input);

        FlightsDto.City Convert(FlightsDomain.Cities input);
    }
}
