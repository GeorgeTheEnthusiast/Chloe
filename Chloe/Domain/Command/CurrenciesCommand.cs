using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Domain.Command
{
    public class CurrenciesCommand : ICurrienciesCommand
    {
        private readonly ICurrencyConverter _currencyConverter;
        
        public CurrenciesCommand(ICurrencyConverter currencyConverter)
        {
            if (currencyConverter == null) throw new ArgumentNullException("currencyConverter");

            _currencyConverter = currencyConverter;
        }

        public FlightsDto.Currency Add(FlightsDto.Currency currency)
        {
            FlightsDto.Currency result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                FlightsDomain.Currencies currencies = _currencyConverter.Convert(currency);
                currencies = flightsEntities.Currencies.Add(currencies);
                result = _currencyConverter.Convert(currencies);
            }

            return result;
        }

        public FlightsDto.Currency Merge(FlightsDto.Currency currency)
        {
            FlightsDto.Currency result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                FlightsDomain.Currencies currencies = _currencyConverter.Convert(currency);

                var existedCurrency = flightsEntities.Currencies
                    .Where(x => x.Name.Trim() == currency.Name.Trim())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedCurrency != null)
                {
                    currencies = existedCurrency;
                }
                else
                {
                    currencies = flightsEntities.Currencies.Add(currencies);
                }
                
                result = _currencyConverter.Convert(currencies);
                flightsEntities.SaveChanges();
            }

            return result;
        }
    }
}
