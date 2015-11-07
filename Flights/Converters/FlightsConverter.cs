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
                .ForMember(x => x.ValidationText, expression => expression.MapFrom(src => src.SearchValidationText))
                .ForMember(x => x.Carriers, expression => expression.MapFrom(src => src.Carrier));

            Mapper.CreateMap<FlightsDomain.Flights, FlightsDto.Flight>()
                .ForMember(x => x.DepartureTime, expression => expression.MapFrom(src => src.DepartureDate))
                .ForMember(x => x.SearchValidationText, expression => expression.MapFrom(src => src.ValidationText))
                .ForMember(x => x.Carrier, expression => expression.MapFrom(src => src.Carriers));

            Mapper.CreateMap<FlightsDto.Currency, FlightsDomain.Currencies>();

            Mapper.CreateMap<FlightsDto.SearchCriteria, FlightsDomain.SearchCriterias>();

            Mapper.CreateMap<FlightsDto.City, FlightsDomain.Cities>();

            Mapper.CreateMap<FlightsDto.Carrier, FlightsDomain.Carriers>();

            Mapper.CreateMap<FlightsDomain.Carriers, FlightsDto.Carrier>();
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
