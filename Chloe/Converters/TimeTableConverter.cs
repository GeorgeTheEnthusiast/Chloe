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
    public class TimeTableConverter : ITimeTableConverter
    {
        public TimeTableConverter()
        {
            Mapper.CreateMap<FlightsDto.TimeTable, FlightsDomain.TimeTable>();

            Mapper.CreateMap<FlightsDomain.TimeTable, FlightsDto.TimeTable>()
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers))
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo))
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.FlightWebsites));

            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>();

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>();

            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>();

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>();

            Mapper.CreateMap<FlightsDto.FlightWebsite, FlightsDomain.FlightWebsites>();

            Mapper.CreateMap<FlightsDomain.FlightWebsites, FlightsDto.FlightWebsite>();
        }
        
        public FlightsDomain.TimeTable Convert(FlightsDto.TimeTable input)
        {
            return Mapper.Map<FlightsDomain.TimeTable>(input);
        }

        public FlightsDto.TimeTable Convert(FlightsDomain.TimeTable input)
        {
            return Mapper.Map<FlightsDto.TimeTable>(input);
        }
    }
}
