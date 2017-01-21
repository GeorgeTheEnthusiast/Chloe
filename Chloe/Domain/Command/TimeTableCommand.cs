using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Domain.Command
{
    public class TimeTableCommand : ITimeTableCommand
    {
        private readonly ITimeTableConverter _timeTableConverter;

        public TimeTableCommand(ITimeTableConverter timeTableConverter)
        {
            if (timeTableConverter == null) throw new ArgumentNullException("timeTableConverter");

            _timeTableConverter = timeTableConverter;
        }

        public FlightsDto.TimeTable Merge(FlightsDto.TimeTable timeTable)
        {
            FlightsDto.TimeTable result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existedTimeTable = flightsEntities.TimeTable
                    .Where(x => timeTable.Carrier.Id == x.Carrier_Id
                   && timeTable.CityFrom.Id == x.CityFrom_Id
                   && timeTable.CityTo.Id == x.CityTo_Id
                   && timeTable.DepartureDate.Year == x.DepartureDate.Year
                   && timeTable.DepartureDate.Month == x.DepartureDate.Month
                   && timeTable.DepartureDate.Day == x.DepartureDate.Day
                   && timeTable.DepartureDate.Hour == x.DepartureDate.Hour
                   && timeTable.DepartureDate.Minute == x.DepartureDate.Minute
                   && timeTable.DepartureDate.Second == x.DepartureDate.Second
                   && timeTable.ArrivalDate.Year == x.ArrivalDate.Year
                   && timeTable.ArrivalDate.Month == x.ArrivalDate.Month
                   && timeTable.ArrivalDate.Day == x.ArrivalDate.Day
                   && timeTable.ArrivalDate.Hour == x.ArrivalDate.Hour
                   && timeTable.ArrivalDate.Minute == x.ArrivalDate.Minute
                   && timeTable.ArrivalDate.Second == x.ArrivalDate.Second)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedTimeTable != null)
                {
                    result = _timeTableConverter.Convert(existedTimeTable);

                    return result;
                }

                FlightsDomain.TimeTable domainTimeTable = _timeTableConverter.Convert(timeTable);
                flightsEntities.TimeTable.Add(domainTimeTable);
                flightsEntities.SaveChanges();
            }

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existedTimeTable = flightsEntities.TimeTable
                    .Where(x => timeTable.Carrier.Id == x.Carrier_Id
                   && timeTable.CityFrom.Id == x.CityFrom_Id
                   && timeTable.CityTo.Id == x.CityTo_Id
                   && timeTable.DepartureDate.Year == x.DepartureDate.Year
                   && timeTable.DepartureDate.Month == x.DepartureDate.Month
                   && timeTable.DepartureDate.Day == x.DepartureDate.Day
                   && timeTable.DepartureDate.Hour == x.DepartureDate.Hour
                   && timeTable.DepartureDate.Minute == x.DepartureDate.Minute
                   && timeTable.DepartureDate.Second == x.DepartureDate.Second
                   && timeTable.ArrivalDate.Year == x.ArrivalDate.Year
                   && timeTable.ArrivalDate.Month == x.ArrivalDate.Month
                   && timeTable.ArrivalDate.Day == x.ArrivalDate.Day
                   && timeTable.ArrivalDate.Hour == x.ArrivalDate.Hour
                   && timeTable.ArrivalDate.Minute == x.ArrivalDate.Minute
                   && timeTable.ArrivalDate.Second == x.ArrivalDate.Second)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();


                result = _timeTableConverter.Convert(existedTimeTable);

                return result;
            }
        }
    }
}
