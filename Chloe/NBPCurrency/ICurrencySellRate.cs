using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;

namespace Flights.NBPCurrency
{
    public interface ICurrencySellRate
    {
        decimal GetSellRate(Currency currency);
    }
}
