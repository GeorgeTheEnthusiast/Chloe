using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDto = Chloe.Dto;
using ChloeDomain = Chloe.Domain.Dto;

namespace Chloe.Converters
{
    public interface IFlightWebsiteConverter
    {
        ChloeDomain.FlightWebsites Convert(ChloeDto.FlightWebsite input);

        ChloeDto.FlightWebsite Convert(ChloeDomain.FlightWebsites input);

        IEnumerable<ChloeDto.FlightWebsite> Convert(IEnumerable<ChloeDomain.FlightWebsites> input);

        IEnumerable<ChloeDto.FlightWebsite> Convert(DbSet<ChloeDomain.FlightWebsites> input);
    }
}
