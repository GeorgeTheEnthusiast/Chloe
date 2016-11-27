using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface ICurrencyConverter
    {
        ChloeDomain.Currencies Convert(ChloeDto.Currency input);

        ChloeDto.Currency Convert(ChloeDomain.Currencies input);
    }
}
