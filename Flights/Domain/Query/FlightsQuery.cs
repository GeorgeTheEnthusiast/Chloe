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
    public class FlightsQuery : IFlightsQuery
    {
        private readonly IFlightsConverter _flightsConverter;

        public FlightsQuery(IFlightsConverter flightsConverter)
        {
            if (flightsConverter == null) throw new ArgumentNullException("flightsConverter");

            _flightsConverter = flightsConverter;
        }

        public List<FlightsDto.Flight> GetAllFLights()
        {
            List<FlightsDto.Flight> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var flights = flightDataModel.Flights
                    .ToList();

                result = _flightsConverter.Convert(flights);
            }

            return result;
        }

        public List<FlightsDto.Flight> GetFlightsBySearchDate(DateTime date)
        {
            List<FlightsDto.Flight> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var flights = flightDataModel.Flights
                    .Where(x => x.SearchDate.Year == date.Year
                   && x.SearchDate.Month == date.Month
                   && x.SearchDate.Day == date.Day)
                    .ToList();

                result = _flightsConverter.Convert(flights);
            }

            return result;
        }
    }
}
