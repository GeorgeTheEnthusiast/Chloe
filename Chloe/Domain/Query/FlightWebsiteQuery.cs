using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;
using Chloe.Dto.Enums;
using Chloe.Exceptions;

namespace Chloe.Domain.Query
{
    public class FlightWebsiteQuery : IFlightWebsiteQuery
    {
        private readonly IFlightWebsiteConverter _flightWebsiteConverter;

        public FlightWebsiteQuery(IFlightWebsiteConverter flightWebsiteConverter)
        {
            if (flightWebsiteConverter == null) throw new ArgumentNullException("flightWebsiteConverter");

            _flightWebsiteConverter = flightWebsiteConverter;
        }

        public IEnumerable<ChloeDto.FlightWebsite> GetAllFlightWebsites()
        {
            IEnumerable<ChloeDto.FlightWebsite> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var flightWebsites = flightDataModel.FlightWebsites;

                result = _flightWebsiteConverter.Convert(flightWebsites);
            }

            return result;
        }

        public ChloeDto.FlightWebsite GetFlightWebsiteByType(FlightWebsite flightWebsite)
        {
            int Id = (int)flightWebsite;
            ChloeDto.FlightWebsite result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var flightWebsiteDomain = ChloeEntities.FlightWebsites
                                        .DefaultIfEmpty(null)
                                        .FirstOrDefault(x => x.Id == Id);

                if (flightWebsiteDomain == null)
                    throw new EntityNotFoundException();

                result = _flightWebsiteConverter.Convert(flightWebsiteDomain);
            }

            return result;
        }
    }
}
