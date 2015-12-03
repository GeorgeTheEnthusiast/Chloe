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
            Mapper.CreateMap<FlightsDto.TimeTable, FlightsDomain.TimeTable>()
                .ForMember(x => x.Carriers, expression => expression.MapFrom(src => src.Carrier))
                .ForMember(x => x.CitiesFrom, expression => expression.MapFrom(src => src.CityFrom))
                .ForMember(x => x.CitiesTo, expression => expression.MapFrom(src => src.CityTo));

            Mapper.CreateMap<FlightsDomain.TimeTable, FlightsDto.TimeTable>()
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers))
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo));

            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>();

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>();

            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>();

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>();
        }
        
        public FlightsDomain.TimeTable Convert(FlightsDto.TimeTable timeTable)
        {
            return Mapper.Map<FlightsDomain.TimeTable>(timeTable);
        }

        public FlightsDto.TimeTable Convert(FlightsDomain.TimeTable timeTable)
        {
            return Mapper.Map<FlightsDto.TimeTable>(timeTable);
        }
    }
}
