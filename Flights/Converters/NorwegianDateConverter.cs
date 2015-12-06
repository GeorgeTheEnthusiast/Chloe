using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace Flights.Converters
{
    public class NorwegianDateConverter : INorwegianDateConverter
    {
        public DateTime Convert(string input)
        {
            input = input.Trim();
            string[] dateSplitted = input.Split(' ');

            int year = int.Parse(dateSplitted[1]);
            int month = ConvertMonth(dateSplitted[0]);

            return new DateTime(year, month, 01);
        }

        private int ConvertMonth(string input)
        {
            switch (input)
            {
                case "stycznia":
                    return 1;
                case "lutego":
                    return 2;
                case "marca":
                    return 3;
                case "kwietnia":
                    return 4;
                case "maja":
                    return 5;
                case "czerwca":
                    return 6;
                case "lipca":
                    return 7;
                case "sierpnia":
                    return 8;
                case "września":
                    return 9;
                case "października":
                    return 10;
                case "listopada":
                    return 11;
                case "grudnia":
                    return 12;
                default:
                    throw new NotSupportedException(string.Format("This date [{0}] is not suppported!", input));
            }
        }
    }
}
