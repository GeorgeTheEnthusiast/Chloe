using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Converters
{
    public interface ICountriesConverter
    {
        List<FlightsDto.Country> Convert(List<FlightsDomain.Countries> countries);
    }
}
