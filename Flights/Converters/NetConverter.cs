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
    public class NetConverter : INetConverter
    {
        public NetConverter()
        {
            Mapper.CreateMap<FlightsDto.Net, FlightsDomain.Net>();

            Mapper.CreateMap<FlightsDomain.Net, FlightsDto.Net>();
        }
        
        public FlightsDomain.Net Convert(FlightsDto.Net net)
        {
            return Mapper.Map<FlightsDomain.Net>(net);
        }

        public FlightsDto.Net Convert(FlightsDomain.Net net)
        {
            return Mapper.Map<FlightsDto.Net>(net);
        }
    }
}
