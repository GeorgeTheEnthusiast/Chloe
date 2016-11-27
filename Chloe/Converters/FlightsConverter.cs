using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public class FlightsConverter : IFlightsConverter
    {
        public FlightsConverter()
        {
            Mapper.CreateMap<ChloeDto.Flight, ChloeDomain.Chloe>()
                .ForMember(x => x.DepartureDate, expression => expression.MapFrom(src => src.DepartureTime));

            Mapper.CreateMap<ChloeDomain.Chloe, ChloeDto.Flight>()
                .ForMember(x => x.DepartureTime, expression => expression.MapFrom(src => src.DepartureDate))
                .ForMember(x => x.Currency, expression => expression.MapFrom(src => src.Currencies))
                .ForMember(x => x.SearchCriteria, expression => expression.MapFrom(src => src.SearchCriterias))
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers));

            Mapper.CreateMap<ChloeDto.Currency, ChloeDomain.Currencies>();

            Mapper.CreateMap<ChloeDomain.Currencies, ChloeDto.Currency>();

            Mapper.CreateMap<ChloeDto.SearchCriteria, ChloeDomain.SearchCriterias>()
                .ForMember(x => x.FlightWebsites, expression => expression.MapFrom(src => src.FlightWebsite));

            Mapper.CreateMap<ChloeDomain.SearchCriterias, ChloeDto.SearchCriteria>()
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.Chloe));

            Mapper.CreateMap<ChloeDto.City, ChloeDomain.Cities>();

            Mapper.CreateMap<ChloeDomain.Cities, ChloeDto.City>();

            Mapper.CreateMap<ChloeDto.Carrier, ChloeDomain.Carriers>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carrier => carrier.Name.Trim()));

            Mapper.CreateMap<ChloeDomain.Carriers, ChloeDto.Carrier>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carrier => carrier.Name.Trim()));

            Mapper.CreateMap<ChloeDto.FlightWebsite, ChloeDomain.FlightWebsites>();

            Mapper.CreateMap<ChloeDomain.FlightWebsites, ChloeDto.FlightWebsite>();
        }

        public ChloeDomain.Chloe Convert(ChloeDto.Flight input)
        {
            return Mapper.Map<ChloeDomain.Chloe>(input);
        }

        public IEnumerable<ChloeDto.Flight> Convert(IEnumerable<ChloeDomain.Chloe> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.Flight>>(input);
        }

        public IEnumerable<ChloeDomain.Chloe> Convert(IEnumerable<ChloeDto.Flight> input)
        {
            return Mapper.Map<IEnumerable<ChloeDomain.Chloe>>(input);
        }

        public IEnumerable<ChloeDto.Flight> Convert(DbSet<ChloeDomain.Chloe> input)
        {
            return Mapper.Map<DbSet<ChloeDto.Flight>>(input);
        }
    }
}
