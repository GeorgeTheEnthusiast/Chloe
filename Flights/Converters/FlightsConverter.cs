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
    public class FlightsConverter : IFlightsConverter
    {
        public FlightsConverter()
        {
            Mapper.CreateMap<FlightsDto.Flight, FlightsDomain.Flights>()
                .ForMember(x => x.DepartureDate, expression => expression.MapFrom(src => src.DepartureTime))
                .ForMember(x => x.ValidationText, expression => expression.MapFrom(src => src.SearchValidationText));

            Mapper.CreateMap<FlightsDomain.Flights, FlightsDto.Flight>()
                .ForMember(x => x.DepartureTime, expression => expression.MapFrom(src => src.DepartureDate))
                .ForMember(x => x.SearchValidationText, expression => expression.MapFrom(src => src.ValidationText))
                .ForMember(x => x.Currency, expression => expression.MapFrom(src => src.Currencies))
                .ForMember(x => x.SearchCriteria, expression => expression.MapFrom(src => src.SearchCriterias));

            Mapper.CreateMap<FlightsDto.Currency, FlightsDomain.Currencies>();

            Mapper.CreateMap<FlightsDomain.Currencies, FlightsDto.Currency>();

            Mapper.CreateMap<FlightsDto.SearchCriteria, FlightsDomain.SearchCriterias>()
                .ForMember(x => x.Carriers, expression => expression.MapFrom(src => src.Carrier));

            Mapper.CreateMap<FlightsDomain.SearchCriterias, FlightsDto.SearchCriteria>()
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers));

            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>()
                .ForMember(x => x.Countries, expression => expression.MapFrom(src => src.Country));

            Mapper.CreateMap<FlightsDomain.Cities, FlightsDto.City>()
                .ForMember(x => x.Country, expression => expression.MapFrom(src => src.Countries));

            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carrier => carrier.Name.Trim()));

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(carrier => carrier.Name.Trim()));

            Mapper.CreateMap<FlightsDto.Country, FlightsDomain.Countries>();

            Mapper.CreateMap<FlightsDomain.Countries, FlightsDto.Country>();
        }

        public FlightsDomain.Flights Convert(FlightsDto.Flight flight)
        {
            return Mapper.Map<FlightsDomain.Flights>(flight);
        }

        public List<FlightsDto.Flight> Convert(List<FlightsDomain.Flights> flight)
        {
            return Mapper.Map<List<FlightsDto.Flight>>(flight);
        }
    }
}
