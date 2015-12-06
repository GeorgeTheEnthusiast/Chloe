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
    public interface IFlightWebsiteConverter
    {
        FlightsDomain.FlightWebsites Convert(FlightsDto.FlightWebsite input);

        FlightsDto.FlightWebsite Convert(FlightsDomain.FlightWebsites input);

        IEnumerable<FlightsDto.FlightWebsite> Convert(IEnumerable<FlightsDomain.FlightWebsites> input);

        IEnumerable<FlightsDto.FlightWebsite> Convert(DbSet<FlightsDomain.FlightWebsites> input);
    }
}
