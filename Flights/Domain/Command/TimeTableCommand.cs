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
                FlightsDomain.TimeTable domainTimeTable = _timeTableConverter.Convert(timeTable);

                var existedTimeTable = flightsEntities.TimeTable
                    .Where(x => CompareDtoToDomain(timeTable, x))
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedTimeTable != null)
                {
                    result = _timeTableConverter.Convert(existedTimeTable);

                    return result;
                }
                
                domainTimeTable = flightsEntities.TimeTable.Add(domainTimeTable);
                flightsEntities.SaveChanges();
            }

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                FlightsDomain.TimeTable domainTimeTable = _timeTableConverter.Convert(timeTable);

                var existedTimeTable = flightsEntities.TimeTable
                    .Where(x => CompareDtoToDomain(timeTable, x))
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                
                result = _timeTableConverter.Convert(existedTimeTable);

                return result;
            }
        }

        private bool CompareDtoToDomain(FlightsDto.TimeTable dtoTimeTable, FlightsDomain.TimeTable domainTimeTable)
        {
            return dtoTimeTable.Carrier.Id == domainTimeTable.Carrier_Id
                   && dtoTimeTable.CityFrom.Id == domainTimeTable.CityFrom_Id
                   && dtoTimeTable.CityTo.Id == domainTimeTable.CityTo_Id
                   && dtoTimeTable.DepartureDate.Year == domainTimeTable.DepartureDate.Year
                   && dtoTimeTable.DepartureDate.Month == domainTimeTable.DepartureDate.Month
                   && dtoTimeTable.DepartureDate.Day == domainTimeTable.DepartureDate.Day
                   && dtoTimeTable.DepartureDate.Hour == domainTimeTable.DepartureDate.Hour
                   && dtoTimeTable.DepartureDate.Minute == domainTimeTable.DepartureDate.Minute
                   && dtoTimeTable.DepartureDate.Second == domainTimeTable.DepartureDate.Second;
        }
    }
}
