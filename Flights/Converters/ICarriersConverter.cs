using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Converters
{
    public interface ICarriersConverter
    {
        FlightsDomain.Carriers Convert(FlightsDto.Carrier carrier);

        FlightsDto.Carrier Convert(FlightsDomain.Carriers carriers);

        IEnumerable<FlightsDto.Carrier> Convert(IEnumerable<FlightsDomain.Carriers> carriers);

        IEnumerable<FlightsDto.Carrier> Convert(DbSet<FlightsDomain.Carriers> carriers);
    }
}
