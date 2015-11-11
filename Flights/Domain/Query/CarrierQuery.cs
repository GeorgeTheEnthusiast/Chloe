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
        private readonly ICarriersConverter _carriersConverter;

        public CarrierQuery(ICarriersConverter carriersConverter)
        {
            if (carriersConverter == null) throw new ArgumentNullException("carriersConverter");

            _carriersConverter = carriersConverter;
        }

        public IEnumerable<FlightsDto.Carrier> GetAllCarriers()
        {
            IEnumerable<FlightsDto.Carrier> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var carriers = flightDataModel.Carriers;
                result = _carriersConverter.Convert(carriers);
            }

            return result;
        }

        public FlightsDto.Carrier GetCarrierByType(CarrierType carrierType)
        {
            int Id = (int)carrierType;
            FlightsDto.Carrier result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var carrierDomain = flightsEntities.Carriers
                                        .DefaultIfEmpty(null)
                                        .FirstOrDefault(x => x.Id == Id);

                if (carrierDomain == null)
                    throw new EntityNotFoundException();

                result = _carriersConverter.Convert(carrierDomain);
            }

            return result;
        }
    }
}
