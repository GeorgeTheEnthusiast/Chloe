using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface ICityConverter
    {
        ChloeDomain.Cities Convert(ChloeDto.City input);

        ChloeDto.City Convert(ChloeDomain.Cities input);
    }
}
