using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Converters
{
    public interface IWizzAirCalendarConverter
    {
        int ConvertMonth(string text);
    }
}
