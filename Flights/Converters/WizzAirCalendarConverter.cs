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
            switch (input.ToUpper())
            {
                case "STYCZEŃ":
                    return 1;
                case "LUTY":
                    return 2;
                case "MARZEC":
                    return 3;
                case "KWIECIEŃ":
                    return 4;
                case "MAJ":
                    return 5;
                case "CZERWIEC":
                    return 6;
                case "LIPIEC":
                    return 7;
                case "SIERPIEŃ":
                    return 8;
                case "WRZESIEŃ":
                    return 9;
                case "PAŹDZIERNIK":
                    return 10;
                case "LISTOPAD":
                    return 11;
                case "GRUDZIEŃ":
                    return 12;
                default:
                    throw new NotSupportedException(string.Format("This month [{0}] is not supported!", input));
            }
        }
    }
}
