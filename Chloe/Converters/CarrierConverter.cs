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
    public class CarrierConverter : ICarrierConverter
    {
        public CarrierConverter()
        {
            Mapper.CreateMap<ChloeDto.Carrier, ChloeDomain.Carriers>();

            Mapper.CreateMap<ChloeDomain.Carriers, ChloeDto.Carrier>();
        }
        
        public ChloeDomain.Carriers Convert(ChloeDto.Carrier input)
        {
            return Mapper.Map<ChloeDomain.Carriers>(input);
        }

        public ChloeDto.Carrier Convert(ChloeDomain.Carriers input)
        {
            return Mapper.Map<ChloeDto.Carrier>(input);
        }
    }
}
