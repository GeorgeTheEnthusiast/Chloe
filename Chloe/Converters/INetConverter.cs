using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface INetConverter
    {
        ChloeDomain.Net Convert(ChloeDto.Net input);

        ChloeDto.Net Convert(ChloeDomain.Net input);
    }
}
