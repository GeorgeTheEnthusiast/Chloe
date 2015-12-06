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
    public class SearchCriteriaConverter : ISearchCriteriaConverter
    {
        public SearchCriteriaConverter()
        {
            Mapper.CreateMap<FlightsDomain.SearchCriterias, FlightsDto.SearchCriteria>()
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo))
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.FlightWebsites))
                .ForMember(x => x.ReceiverGroup, expression => expression.MapFrom(src => src.ReceiverGroups));

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>();

            Mapper.CreateMap<FlightsDomain.ReceiverGroups, FlightsDto.ReceiverGroup>();

            Mapper.CreateMap<FlightsDomain.FlightWebsites, FlightsDto.FlightWebsite>();
        }

        public FlightsDto.SearchCriteria Convert(FlightsDomain.SearchCriterias input)
        {
            return Mapper.Map<FlightsDto.SearchCriteria>(input);
        }

        public IEnumerable<FlightsDto.SearchCriteria> Convert(IEnumerable<FlightsDomain.SearchCriterias> input)
        {
            return Mapper.Map<IEnumerable<FlightsDto.SearchCriteria>>(input);
        }
    }
}
