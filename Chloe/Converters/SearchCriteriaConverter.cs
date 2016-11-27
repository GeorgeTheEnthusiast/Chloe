using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public class SearchCriteriaConverter : ISearchCriteriaConverter
    {
        public SearchCriteriaConverter()
        {
            Mapper.CreateMap<ChloeDomain.SearchCriterias, ChloeDto.SearchCriteria>()
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo))
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.FlightWebsites))
                .ForMember(x => x.ReceiverGroup, expression => expression.MapFrom(src => src.ReceiverGroups));

            Mapper.CreateMap<ChloeDomain.Cities, ChloeDto.City>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<ChloeDomain.Carriers, ChloeDto.Carrier>();

            Mapper.CreateMap<ChloeDomain.ReceiverGroups, ChloeDto.ReceiverGroup>();

            Mapper.CreateMap<ChloeDomain.FlightWebsites, ChloeDto.FlightWebsite>();
        }

        public ChloeDto.SearchCriteria Convert(ChloeDomain.SearchCriterias input)
        {
            return Mapper.Map<ChloeDto.SearchCriteria>(input);
        }

        public IEnumerable<ChloeDto.SearchCriteria> Convert(IEnumerable<ChloeDomain.SearchCriterias> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.SearchCriteria>>(input);
        }
    }
}
