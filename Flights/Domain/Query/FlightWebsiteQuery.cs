using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;
using Flights.Dto.Enums;
using Flights.Exceptions;

namespace Flights.Domain.Query
{
    public class FlightWebsiteQuery : IFlightWebsiteQuery
    {
        private readonly IFlightWebsiteConverter _flightWebsiteConverter;

        public FlightWebsiteQuery(IFlightWebsiteConverter flightWebsiteConverter)
        {
            if (flightWebsiteConverter == null) throw new ArgumentNullException("flightWebsiteConverter");

            _flightWebsiteConverter = flightWebsiteConverter;
        }

        public IEnumerable<FlightsDto.FlightWebsite> GetAllFlightWebsites()
        {
            IEnumerable<FlightsDto.FlightWebsite> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var flightWebsites = flightDataModel.FlightWebsites;

                result = _flightWebsiteConverter.Convert(flightWebsites);
            }

            return result;
        }

        public FlightsDto.FlightWebsite GetFlightWebsiteByType(FlightWebsite flightWebsite)
        {
            int Id = (int)flightWebsite;
            FlightsDto.FlightWebsite result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var flightWebsiteDomain = flightsEntities.FlightWebsites
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
