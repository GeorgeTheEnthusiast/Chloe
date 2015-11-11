using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;
using Flights.Dto.Enums;

namespace Flights.Domain.Query
{
    public interface ICarrierQuery
    {
        IEnumerable<FlightsDto.Carrier> GetAllCarriers();

        FlightsDto.Carrier GetCarrierByType(CarrierType carrierType);
    }
}
