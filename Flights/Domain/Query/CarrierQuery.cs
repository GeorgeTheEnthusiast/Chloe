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
    public class CarrierQuery : ICarrierQuery
    {
        private readonly ICarrierConverter _carrierConverter;

        public CarrierQuery(ICarrierConverter carrierConverter)
        {
            if (carrierConverter == null) throw new ArgumentNullException("carrierConverter");

            _carrierConverter = carrierConverter;
        }

        public FlightsDto.Carrier GetCarrierByName(string name)
        {
            FlightsDto.Carrier result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var carrierDomain = flightsEntities.Carriers
                                        .DefaultIfEmpty(null)
                                        .FirstOrDefault(x => x.Name.Trim() == name.Trim());

                if (carrierDomain == null)
                    throw new EntityNotFoundException();

                result = _carrierConverter.Convert(carrierDomain);
            }

            return result;
        }
    }
}
