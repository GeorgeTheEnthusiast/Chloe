using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Flights.Domain.Dto;
using FlightDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ITimeTableStatusConverter
    {
        IEnumerable<FlightDto.TimeTableStatus> Convert(IEnumerable<FlightDomain.TimeTableStatus> input);

        FlightDomain.TimeTableStatus Convert(FlightDto.TimeTableStatus input);

        FlightDto.TimeTableStatus Convert(FlightDomain.TimeTableStatus input);
    }
}
