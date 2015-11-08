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
        
        public List<FlightsDto.Country> Convert(List<FlightsDomain.Countries> countries)
        {
            return Mapper.Map<List<FlightsDto.Country>>(countries);
        }
    }
}
