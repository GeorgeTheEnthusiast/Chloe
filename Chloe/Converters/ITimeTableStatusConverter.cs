using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Chloe.Domain.Dto;
using FlightDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface ITimeTableStatusConverter
    {
        IEnumerable<FlightDto.TimeTableStatus> Convert(IEnumerable<FlightDomain.TimeTableStatus> input);

        FlightDomain.TimeTableStatus Convert(FlightDto.TimeTableStatus input);

        FlightDto.TimeTableStatus Convert(FlightDomain.TimeTableStatus input);
    }
}
