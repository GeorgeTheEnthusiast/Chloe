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
    public interface IFlightsQuery
    {
        IEnumerable<FlightsDto.Flight> GetAllFlights();

        IEnumerable<FlightsDto.Flight> GetFlightsBySearchDate(DateTime date);

        IEnumerable<FlightsDto.Flight> GetFlightsByReceiverGroup(FlightsDto.ReceiverGroup receiverGroup);
    }
}
