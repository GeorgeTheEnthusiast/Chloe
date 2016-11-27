using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDto = Chloe.Dto;
using ChloeDomain = Chloe.Domain.Dto;

namespace Chloe.Domain.Command
{
    public class TimeTableStatusCommand : ITimeTableStatusCommand
    {
        private readonly ITimeTableStatusConverter _timeTableStatusConverter;

        public TimeTableStatusCommand(ITimeTableStatusConverter timeTableStatusConverter)
        {
            if (timeTableStatusConverter == null) throw new ArgumentNullException("timeTableStatusConverter");

            _timeTableStatusConverter = timeTableStatusConverter;
        }

        public ChloeDto.TimeTableStatus Merge(ChloeDto.TimeTableStatus input)
        {
			ChloeDto.TimeTableStatus output;
			
            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var existed = ChloeEntities.TimeTableStatus
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
						ChloeEntities.SaveChanges();
					}
                }
                else
                {
                    var domainTimeTableStatus = _timeTableStatusConverter.Convert(input);
                    ChloeEntities.TimeTableStatus.Add(domainTimeTableStatus);
                    ChloeEntities.SaveChanges();
                }
            }
			
			using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var existed = ChloeEntities.TimeTableStatus
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
