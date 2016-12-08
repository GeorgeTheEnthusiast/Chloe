using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Exceptions;
using NLog;
using FlightDataModel = Flights.Domain.Dto;
using FlightDto = Flights.Dto;

namespace Flights.Domain.Query
{
    public class TimeTableStatusQuery : ITimeTableStatusQuery
    {
        private readonly ITimeTableStatusConverter _timeTableStatusConverter;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public TimeTableStatusQuery(ITimeTableStatusConverter timeTableStatusConverter)
        {
            if (timeTableStatusConverter == null) throw new ArgumentNullException("timeTableStatusConverter");

            _timeTableStatusConverter = timeTableStatusConverter;
        }

        public IEnumerable<FlightDto.TimeTableStatus> GetTimeTableStatusesByWebSiteId(int id)
        {
            IEnumerable<FlightDto.TimeTableStatus> result;

            using (FlightDataModel.FlightsEntities flightDataModel = new FlightDataModel.FlightsEntities())
            {
                var domainModel = flightDataModel.TimeTableStatus
                    .Where(x => x.FlightWebsite_Id == id)
                    .ToList();
                result = _timeTableStatusConverter.Convert(domainModel);
            }

            return result;
        }

        public bool IsTimeTableFresh(FlightDto.TimeTableStatus timeTableStatus)
        {
            using (FlightDataModel.FlightsEntities flightDataModel = new FlightDataModel.FlightsEntities())
            {
                var domainModel = flightDataModel.TimeTableStatus.FirstOrDefault(x => x.FlightWebsite_Id == timeTableStatus.FlightWebsite.Id
                                                                                      && x.CityFrom_Id == timeTableStatus.CityFrom.Id
                                                                                      && x.CityTo_Id == timeTableStatus.CityTo.Id);

                if (domainModel != null)
                {
                    if (domainModel.SearchDate == null
                        || DateTime.Compare(domainModel.SearchDate.Value.Date, DateTime.Now.AddMonths(-3).Date) <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    _logger.Error("TimeTableStatus with flightWebsite_Id [{0}], CityFrom_Id [{1}] and CityTo_Id [{2}] not found!", timeTableStatus.FlightWebsite.Id, timeTableStatus.CityFrom.Id, timeTableStatus.CityTo.Id);
                    throw new EntityNotFoundException();
                }
            }
        }
    }
}
