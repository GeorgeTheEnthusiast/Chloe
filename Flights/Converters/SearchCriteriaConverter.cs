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
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CityFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CityTo))
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers));

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>();
        }

        public FlightsDto.SearchCriteria Convert(FlightsDomain.SearchCriterias searchCriteria)
        {
            return Mapper.Map<FlightsDto.SearchCriteria>(searchCriteria);
        }

        public IEnumerable<FlightsDto.SearchCriteria> Convert(IEnumerable<FlightsDomain.SearchCriterias> searchCriteria)
        {
            return Mapper.Map<IEnumerable<FlightsDto.SearchCriteria>>(searchCriteria);
        }

        public List<FlightsDto.SearchCriteria> Convert(List<FlightsDomain.SearchCriterias> searchCriteria)
        {
            return Mapper.Map<List<FlightsDto.SearchCriteria>>(searchCriteria);
        }
    }
}
