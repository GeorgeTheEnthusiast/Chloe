using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Command
{
    public class FlightsCommand : IFlightsCommand
    {
        private readonly IFlightsConverter _flightsConverter;
        
        public FlightsCommand(IFlightsConverter flightsConverter)
        {
            if (flightsConverter == null) throw new ArgumentNullException("flightsConverter");

            _flightsConverter = flightsConverter;
        }

        public void Add(FlightsDto.Flight flight)
        {
            using (var flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var domainFlights = _flightsConverter.Convert(flight);
                flightsEntities.Flights.Add(domainFlights);
                flightsEntities.SaveChanges();
            }
        }
    }
}
