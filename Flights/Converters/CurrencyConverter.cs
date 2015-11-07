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
    public class CurrencyConverter : ICurrencyConverter
    {
        public CurrencyConverter()
        {
            Mapper.CreateMap<FlightsDto.Currency, FlightsDomain.Currencies>();

            Mapper.CreateMap<FlightsDomain.Currencies, FlightsDto.Currency>();
        }

        public FlightsDomain.Currencies Convert(FlightsDto.Currency currency)
        {
            return Mapper.Map<FlightsDomain.Currencies>(currency);
        }

        public FlightsDto.Currency Convert(FlightsDomain.Currencies currency)
        {
            return Mapper.Map<FlightsDto.Currency>(currency);
        }
    }
}
