using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ICarrierConverter
    {
        FlightsDomain.Carriers Convert(FlightsDto.Carrier carrier);

        FlightsDto.Carrier Convert(FlightsDomain.Carriers carriers);
    }
}
