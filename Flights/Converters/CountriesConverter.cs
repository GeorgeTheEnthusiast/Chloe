using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public class CountriesConverter : ICountriesConverter
    {
        public CountriesConverter()
        {
            Mapper.CreateMap<FlightsDomain.Countries, FlightsDto.Country>();
        }
        
        public IEnumerable<FlightsDto.Country> Convert(IEnumerable<FlightsDomain.Countries> countries)
        {
            return Mapper.Map<IEnumerable<FlightsDto.Country>>(countries);
        }
    }
}
