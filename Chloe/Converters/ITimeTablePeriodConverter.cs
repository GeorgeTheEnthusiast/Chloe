using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface ITimeTablePeriodConverter
    {
        IEnumerable<DateTime> Convert(IEnumerable<int> daysInWeek, DateTime dateFrom, DateTime dateTo);

        int ConvertDay(string day);
    }
}
