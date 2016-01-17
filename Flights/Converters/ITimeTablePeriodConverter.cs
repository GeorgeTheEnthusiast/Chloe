using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ITimeTablePeriodConverter
    {
        IEnumerable<DateTime> Convert(IEnumerable<int> daysInWeek, DateTime dateFrom, DateTime dateTo);

        int ConvertDay(string day);
    }
}
