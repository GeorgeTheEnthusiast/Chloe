using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Domain.Query;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Domain.Command
{
    public class CarrierCommand : ICarrierCommand
    {
        private readonly ICarrierConverter _carrierConverter;

        public CarrierCommand(ICarrierConverter carrierConverter)
        {
            if (carrierConverter == null) throw new ArgumentNullException("carrierConverter");

            _carrierConverter = carrierConverter;
        }

        public FlightsDto.Carrier Merge(string name)
        {
            FlightsDto.Carrier result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existedCarrier = flightsEntities.Carriers
                    .Where(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedCarrier != null)
                {
                    result = _carrierConverter.Convert(existedCarrier);

                    return result;
                }
                else
                {
                    flightsEntities.Carriers.Add(new FlightsDomain.Carriers()
                    {
                      Name  = name
                    });
                }
                
                flightsEntities.SaveChanges();
            }

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existedCarrier = flightsEntities.Carriers
                    .Where(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                result = _carrierConverter.Convert(existedCarrier);
            }

            return result;
        }
    }
}
