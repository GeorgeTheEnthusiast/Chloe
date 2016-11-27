using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface ICarrierConverter
    {
        ChloeDomain.Carriers Convert(ChloeDto.Carrier input);

        ChloeDto.Carrier Convert(ChloeDomain.Carriers input);
    }
}
