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
    public class CarrierConverter : ICarrierConverter
    {
        public CarrierConverter()
        {
            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>();

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>();
        }
        
        public FlightsDomain.Carriers Convert(FlightsDto.Carrier input)
        {
            return Mapper.Map<FlightsDomain.Carriers>(input);
        }

        public FlightsDto.Carrier Convert(FlightsDomain.Carriers input)
        {
            return Mapper.Map<FlightsDto.Carrier>(input);
        }
    }
}
