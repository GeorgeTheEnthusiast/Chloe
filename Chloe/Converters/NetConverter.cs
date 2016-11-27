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
    public class NetConverter : INetConverter
    {
        public NetConverter()
        {
            Mapper.CreateMap<ChloeDto.Net, ChloeDomain.Net>();

            Mapper.CreateMap<ChloeDomain.Net, ChloeDto.Net>();
        }
        
        public ChloeDomain.Net Convert(ChloeDto.Net input)
        {
            return Mapper.Map<ChloeDomain.Net>(input);
        }

        public ChloeDto.Net Convert(ChloeDomain.Net input)
        {
            return Mapper.Map<ChloeDto.Net>(input);
        }
    }
}
