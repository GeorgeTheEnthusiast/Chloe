﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Converters
{
    public class CarriersConverter : ICarriersConverter
    {
        public CarriersConverter()
        {
            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>();

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carriers => carriers.Name.Trim()));
        }

        public FlightsDomain.Carriers Convert(FlightsDto.Carrier carrier)
        {
            return Mapper.Map<FlightsDomain.Carriers>(carrier);
        }

        public FlightsDto.Carrier Convert(FlightsDomain.Carriers carriers)
        {
            return Mapper.Map<FlightsDto.Carrier>(carriers);
        }

        public List<FlightsDto.Carrier> Convert(List<FlightsDomain.Carriers> carriers)
        {
            return Mapper.Map<List<FlightsDto.Carrier>>(carriers);
        }
    }
}
