using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Dto;

namespace Flights.Domain.Query
{
    public class CurrencyQuery : ICurrencyQuery
    {
        public List<Currencies> GetAllCurrencies()
        {
            List<Currencies> result;

            using (var flightDataModel = new FlightsEntities())
            {
                result = flightDataModel.Currencies.ToList();
            }

            return result;
        }
    }
}
