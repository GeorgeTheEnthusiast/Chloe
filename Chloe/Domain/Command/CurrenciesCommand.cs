using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDto = Chloe.Dto;
using ChloeDomain = Chloe.Domain.Dto;

namespace Chloe.Domain.Command
{
    public class CurrenciesCommand : ICurrienciesCommand
    {
        private readonly ICurrencyConverter _currencyConverter;
        
        public CurrenciesCommand(ICurrencyConverter currencyConverter)
        {
            if (currencyConverter == null) throw new ArgumentNullException("currencyConverter");

            _currencyConverter = currencyConverter;
        }

        public ChloeDto.Currency Add(ChloeDto.Currency currency)
        {
            ChloeDto.Currency result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                ChloeDomain.Currencies currencies = _currencyConverter.Convert(currency);
                currencies = ChloeEntities.Currencies.Add(currencies);
                result = _currencyConverter.Convert(currencies);
            }

            return result;
        }

        public ChloeDto.Currency Merge(ChloeDto.Currency currency)
        {
            ChloeDto.Currency result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                ChloeDomain.Currencies currencies = _currencyConverter.Convert(currency);

                var existedCurrency = ChloeEntities.Currencies
                    .Where(x => x.Name.Trim() == currency.Name.Trim())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedCurrency != null)
                {
                    currencies = existedCurrency;
                }
                else
                {
                    currencies = ChloeEntities.Currencies.Add(currencies);
                }
                
                result = _currencyConverter.Convert(currencies);
                ChloeEntities.SaveChanges();
            }

            return result;
        }
    }
}
