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
    public class TimeTableStatusCommand : ITimeTableStatusCommand
    {
        private readonly ITimeTableStatusConverter _timeTableStatusConverter;

        public TimeTableStatusCommand(ITimeTableStatusConverter timeTableStatusConverter)
        {
            if (timeTableStatusConverter == null) throw new ArgumentNullException("timeTableStatusConverter");

            _timeTableStatusConverter = timeTableStatusConverter;
        }

        public FlightsDto.TimeTableStatus Merge(FlightsDto.TimeTableStatus input)
        {
			FlightsDto.TimeTableStatus output;
			
            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existed = flightsEntities.TimeTableStatus
                    .Where(x => input.FlightWebsite.Id == x.FlightWebsite_Id
                                && input.CityFrom.Id == x.CityFrom_Id
                                && input.CityTo.Id == x.CityTo_Id)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existed != null)
				{
					if (input.SearchDate != null)
					{
						existed.SearchDate = input.SearchDate;
						flightsEntities.SaveChanges();
					}
                }
                else
                {
                    var domainTimeTableStatus = _timeTableStatusConverter.Convert(input);
                    flightsEntities.TimeTableStatus.Add(domainTimeTableStatus);
                    flightsEntities.SaveChanges();
                }
            }
			
			using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existed = flightsEntities.TimeTableStatus
                    .Where(x => input.FlightWebsite.Id == x.FlightWebsite_Id
                                && input.CityFrom.Id == x.CityFrom_Id
                                && input.CityTo.Id == x.CityTo_Id)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                output = _timeTableStatusConverter.Convert(existed);
				return output;
            }
        }
    }
}
