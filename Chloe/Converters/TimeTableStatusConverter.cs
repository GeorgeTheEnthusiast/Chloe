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
    public class TimeTableStatusConverter : ITimeTableStatusConverter
    {
        public TimeTableStatusConverter()
        {
            Mapper.CreateMap<ChloeDto.TimeTableStatus, ChloeDomain.TimeTableStatus>();

            Mapper.CreateMap<ChloeDomain.TimeTableStatus, ChloeDto.TimeTableStatus>()
                .ForMember(x => x.CityFrom, expression => expression.MapFrom(src => src.CitiesFrom))
                .ForMember(x => x.CityTo, expression => expression.MapFrom(src => src.CitiesTo))
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.FlightWebsites));

            Mapper.CreateMap<ChloeDomain.Cities, ChloeDto.City>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<ChloeDto.City, ChloeDomain.Cities>()
                .ForMember(x => x.Name, expression => expression.MapFrom(src => src.Name.Trim()));

            Mapper.CreateMap<ChloeDomain.FlightWebsites, ChloeDto.FlightWebsite>();

            Mapper.CreateMap<ChloeDto.FlightWebsite, ChloeDomain.FlightWebsites>();
        }

        public IEnumerable<ChloeDto.TimeTableStatus> Convert(IEnumerable<ChloeDomain.TimeTableStatus> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.TimeTableStatus>>(input);
        }

        public ChloeDomain.TimeTableStatus Convert(ChloeDto.TimeTableStatus input)
        {
            return Mapper.Map<ChloeDomain.TimeTableStatus>(input);
        }

        public ChloeDto.TimeTableStatus Convert(ChloeDomain.TimeTableStatus input)
        {
            return Mapper.Map<ChloeDto.TimeTableStatus>(input);
        }
    }
}
