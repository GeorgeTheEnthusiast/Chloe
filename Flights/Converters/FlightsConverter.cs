using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public class FlightsConverter : IFlightsConverter
    {
        public FlightsConverter()
        {
            Mapper.CreateMap<FlightsDto.Flight, FlightsDomain.Flights>()
                .ForMember(x => x.DepartureDate, expression => expression.MapFrom(src => src.DepartureTime));

            Mapper.CreateMap<FlightsDomain.Flights, FlightsDto.Flight>()
                .ForMember(x => x.DepartureTime, expression => expression.MapFrom(src => src.DepartureDate))
                .ForMember(x => x.Currency, expression => expression.MapFrom(src => src.Currencies))
                .ForMember(x => x.SearchCriteria, expression => expression.MapFrom(src => src.SearchCriterias))
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers));

            Mapper.CreateMap<FlightsDto.Currency, FlightsDomain.Currencies>();

            Mapper.CreateMap<FlightsDomain.Currencies, FlightsDto.Currency>();

            Mapper.CreateMap<FlightsDto.SearchCriteria, FlightsDomain.SearchCriterias>()
                .ForMember(x => x.FlightWebsites, expression => expression.MapFrom(src => src.FlightWebsite));

            Mapper.CreateMap<FlightsDomain.SearchCriterias, FlightsDto.SearchCriteria>()
                .ForMember(x => x.FlightWebsite, expression => expression.MapFrom(src => src.Flights));

            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>();

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>();

            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carrier => carrier.Name.Trim()));

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carrier => carrier.Name.Trim()));

            Mapper.CreateMap<FlightsDto.FlightWebsite, FlightsDomain.FlightWebsites>();

            Mapper.CreateMap<FlightsDomain.FlightWebsites, FlightsDto.FlightWebsite>();
        }

        public FlightsDomain.Flights Convert(FlightsDto.Flight input)
        {
            return Mapper.Map<FlightsDomain.Flights>(input);
        }

        public IEnumerable<FlightsDto.Flight> Convert(IEnumerable<FlightsDomain.Flights> input)
        {
            return Mapper.Map<IEnumerable<FlightsDto.Flight>>(input);
        }

        public IEnumerable<FlightsDomain.Flights> Convert(IEnumerable<FlightsDto.Flight> input)
        {
            return Mapper.Map<IEnumerable<FlightsDomain.Flights>>(input);
        }

        public IEnumerable<FlightsDto.Flight> Convert(DbSet<FlightsDomain.Flights> input)
        {
            return Mapper.Map<DbSet<FlightsDto.Flight>>(input);
        }
    }
}
