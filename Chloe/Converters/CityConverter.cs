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
    public class CityConverter : ICityConverter
    {
        public CityConverter()
        {
            Mapper.CreateMap<ChloeDto.City, ChloeDomain.Cities>();

            Mapper.CreateMap<ChloeDomain.Cities, ChloeDto.City>();
        }
        
        public ChloeDomain.Cities Convert(ChloeDto.City input)
        {
            return Mapper.Map<ChloeDomain.Cities>(input);
        }

        public ChloeDto.City Convert(ChloeDomain.Cities input)
        {
            return Mapper.Map<ChloeDto.City>(input);
        }
    }
}
