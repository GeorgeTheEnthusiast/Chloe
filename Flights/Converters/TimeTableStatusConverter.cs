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
    public class TimeTableStatusConverter : ITimeTableStatusConverter
    {
        public TimeTableStatusConverter()
        {
            Mapper.CreateMap<FlightsDto.TimeTableStatus, FlightsDomain.TimeTableStatus>();

            Mapper.CreateMap<FlightsDomain.TimeTableStatus, FlightsDto.TimeTableStatus>()
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo))
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.FlightWebsites));

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<FlightsDomain.FlightWebsites, FlightsDto.FlightWebsite>();

            Mapper.CreateMap<FlightsDto.FlightWebsite, FlightsDomain.FlightWebsites>();
        }

        public IEnumerable<FlightsDto.TimeTableStatus> Convert(IEnumerable<FlightsDomain.TimeTableStatus> input)
        {
            return Mapper.Map<IEnumerable<FlightsDto.TimeTableStatus>>(input);
        }

        public FlightsDomain.TimeTableStatus Convert(FlightsDto.TimeTableStatus input)
        {
            return Mapper.Map<FlightsDomain.TimeTableStatus>(input);
        }

        public FlightsDto.TimeTableStatus Convert(FlightsDomain.TimeTableStatus input)
        {
            return Mapper.Map<FlightsDto.TimeTableStatus>(input);
        }
    }
}
