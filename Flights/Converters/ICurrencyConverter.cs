using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ICurrencyConverter
    {
        FlightsDomain.Currencies Convert(FlightsDto.Currency currency);

        FlightsDto.Currency Convert(FlightsDomain.Currencies currency);
    }
}
