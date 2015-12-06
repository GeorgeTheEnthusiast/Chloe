using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Converters
{
    public class WizzAirCalendarConverter : IWizzAirCalendarConverter
    {
        public int ConvertMonth(string input)
        {
            switch (input)
            {
                case "Styczeń":
                    return 1;
                case "Luty":
                    return 2;
                case "Marzec":
                    return 3;
                case "Kwiecień":
                    return 4;
                case "Maj":
                    return 5;
                case "Czerwiec":
                    return 6;
                case "Lipiec":
                    return 7;
                case "Sierpień":
                    return 8;
                case "Wrzesień":
                    return 9;
                case "Październik":
                    return 10;
                case "Listopad":
                    return 11;
                case "Grudzień":
                    return 12;
                default:
                    throw new NotSupportedException(string.Format("This month [{0}] is not supported!", input));
            }
        }
    }
}
