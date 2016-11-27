using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface IFlightsConverter
    {
        ChloeDomain.Chloe Convert(ChloeDto.Flight input);

        IEnumerable<ChloeDto.Flight> Convert(IEnumerable<ChloeDomain.Chloe> input);

        IEnumerable<ChloeDomain.Chloe> Convert(IEnumerable<ChloeDto.Flight> input);

        IEnumerable<ChloeDto.Flight> Convert(DbSet<ChloeDomain.Chloe> input);
    }
}
