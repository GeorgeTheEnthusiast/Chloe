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
    public class TimeTableConverter : ITimeTableConverter
    {
        public TimeTableConverter()
        {
            Mapper.CreateMap<ChloeDto.TimeTable, ChloeDomain.TimeTable>();

            Mapper.CreateMap<ChloeDomain.TimeTable, ChloeDto.TimeTable>()
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers))
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo))
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.FlightWebsites));

            Mapper.CreateMap<ChloeDto.Carrier, ChloeDomain.Carriers>();

            Mapper.CreateMap<ChloeDomain.Carriers, ChloeDto.Carrier>();

            Mapper.CreateMap<ChloeDto.City, ChloeDomain.Cities>();

            Mapper.CreateMap<ChloeDomain.Cities, ChloeDto.City>();

            Mapper.CreateMap<ChloeDto.FlightWebsite, ChloeDomain.FlightWebsites>();

            Mapper.CreateMap<ChloeDomain.FlightWebsites, ChloeDto.FlightWebsite>();
        }
        
        public ChloeDomain.TimeTable Convert(ChloeDto.TimeTable input)
        {
            return Mapper.Map<ChloeDomain.TimeTable>(input);
        }

        public ChloeDto.TimeTable Convert(ChloeDomain.TimeTable input)
        {
            return Mapper.Map<ChloeDto.TimeTable>(input);
        }
    }
}
