using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public class TimeTablePeriodConverter : ITimeTablePeriodConverter
    {
        public IEnumerable<DateTime> Convert(IEnumerable<int> daysInWeek, DateTime dateFrom, DateTime dateTo)
        {
            List<DateTime> output = new List<DateTime>();

            foreach (var day in daysInWeek)
            {
                if (day <= 0 || day > 7)
                    throw new NotSupportedException(string.Format("This day [{0}] is not supported!", day));

                DateTime tempDate = dateFrom.Date;
                int tempDay = ConvertDay(tempDate);

                while (tempDay != day)
                {
                    tempDate = tempDate.AddDays(1);
                    tempDay = ConvertDay(tempDate);
                }

                while (DateTime.Compare(tempDate.Date, dateTo.Date) <= 0)
                {
                    output.Add(tempDate);
                    tempDate = tempDate.AddDays(7);
                }
            }

            output.Sort();
            return output;
        }

        public int ConvertDay(string day)
        {
            switch (day.Trim().ToLower())
            {
                case "poniedziałek":
                    return 1;
                case "wtorek":
                    return 2;
                case "środa":
                    return 3;
                case "czwartek":
                    return 4;
                case "piątek":
                    return 5;
                case "sobota":
                    return 6;
                case "niedziela":
                    return 7;
                default:
                    throw new NotSupportedException(string.Format("This type of day [{0}] is not supported!", day));
            }
        }

        private int ConvertDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default:
                    throw new NotSupportedException(string.Format("This type of week [{0}] is not supported!", date.DayOfWeek));
            }
        }
    }
}
