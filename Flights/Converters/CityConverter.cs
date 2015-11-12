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
    public class CityConverter : ICityConverter
    {
        public CityConverter()
        {
            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>();

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>();
        }
        
        public FlightsDomain.Cities Convert(FlightsDto.City city)
        {
            return Mapper.Map<FlightsDomain.Cities>(city);
        }

        public FlightsDto.City Convert(FlightsDomain.Cities city)
        {
            return Mapper.Map<FlightsDto.City>(city);
        }
    }
}
