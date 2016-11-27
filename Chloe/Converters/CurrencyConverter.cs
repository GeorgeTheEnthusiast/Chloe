using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public class CurrencyConverter : ICurrencyConverter
    {
        public CurrencyConverter()
        {
            Mapper.CreateMap<ChloeDto.Currency, ChloeDomain.Currencies>();

            Mapper.CreateMap<ChloeDomain.Currencies, ChloeDto.Currency>();
        }

        public ChloeDomain.Currencies Convert(ChloeDto.Currency input)
        {
            return Mapper.Map<ChloeDomain.Currencies>(input);
        }

        public ChloeDto.Currency Convert(ChloeDomain.Currencies input)
        {
            return Mapper.Map<ChloeDto.Currency>(input);
        }
    }
}
