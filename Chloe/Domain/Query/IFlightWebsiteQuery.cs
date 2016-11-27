using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;
using Chloe.Dto.Enums;

namespace Chloe.Domain.Query
{
    public interface IFlightWebsiteQuery
    {
        IEnumerable<ChloeDto.FlightWebsite> GetAllFlightWebsites();

        ChloeDto.FlightWebsite GetFlightWebsiteByType(FlightWebsite flightWebsite);
    }
}
