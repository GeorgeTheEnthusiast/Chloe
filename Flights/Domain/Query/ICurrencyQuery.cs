using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Dto;

namespace Flights.Domain.Query
{
    public interface ICurrencyQuery
    {
        List<Currencies> GetAllCurrencies();
    }
}
