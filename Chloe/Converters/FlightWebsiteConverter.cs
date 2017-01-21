using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Converters
{
    public class FlightWebsiteConverter : IFlightWebsiteConverter
    {
        public FlightWebsiteConverter()
        {
            Mapper.CreateMap<FlightsDto.FlightWebsite, FlightsDomain.FlightWebsites>();

            Mapper.CreateMap<FlightsDomain.FlightWebsites, FlightsDto.FlightWebsite>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(flightWebsite => flightWebsite.Name.Trim()));
        }
        
        public FlightsDomain.FlightWebsites Convert(FlightsDto.FlightWebsite input)
        {
            return Mapper.Map<FlightsDomain.FlightWebsites>(input);
        }

        public FlightsDto.FlightWebsite Convert(FlightsDomain.FlightWebsites input)
        {
            return Mapper.Map<FlightsDto.FlightWebsite>(input);
        }

        public IEnumerable<FlightsDto.FlightWebsite> Convert(IEnumerable<FlightsDomain.FlightWebsites> input)
        {
            return Mapper.Map<IEnumerable<FlightsDto.FlightWebsite>>(input);
        }

        public IEnumerable<FlightsDto.FlightWebsite> Convert(DbSet<FlightsDomain.FlightWebsites> input)
        {
            return Mapper.Map<IEnumerable<FlightsDto.FlightWebsite>>(input);
        }
    }
}
