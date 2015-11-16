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

        public void DeleteFlightsBySearchDate(DateTime date)
        {
            using (var flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var flightsToDelete = flightsEntities.Flights.Where(x => x.SearchDate.Year == date.Year
                                                                         && x.SearchDate.Month == date.Month
                                                                         && x.SearchDate.Day == date.Day);
                if (flightsToDelete.Any())
                {
                    flightsEntities.Flights.RemoveRange(flightsToDelete);
                    flightsEntities.SaveChanges();
                }
            }
        }

        public void AddRange(IEnumerable<FlightsDto.Flight> flights)
        {
            using (var flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var domainFlights = _flightsConverter.Convert(flights);
                flightsEntities.Flights.AddRange(domainFlights);
                flightsEntities.SaveChanges();
            }
        }

        public void DeleteFlightsBySearchCriteria(FlightsDto.SearchCriteria searchCriteria)
        {
            using (var flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var flightsToDelete = flightsEntities.Flights.Where(x => x.SearchCriterias.Id == searchCriteria.Id);

                if (flightsToDelete.Any())
                {
                    flightsEntities.Flights.RemoveRange(flightsToDelete);
                    flightsEntities.SaveChanges();
                }
            }
        }
    }
}
