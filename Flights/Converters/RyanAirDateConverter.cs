using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Converters
{
    public class RyanAirDateConverter : IRyanAirDateConverter
    {
        public DateTime Convert(DateTime dateToMergeWith, string ryanAirDate)
        {
            string[] splitted = ryanAirDate.Split(' ');
            int year = dateToMergeWith.Year;
            int month = GetMonth(splitted[2]);
            int day = int.Parse(splitted[1]);

            return new DateTime(year, month, day, dateToMergeWith.Hour, dateToMergeWith.Minute, dateToMergeWith.Second);
        }

        private int GetMonth(string month)
        {
            switch (month)
            {
                case "sty":
                    return 1;
                case "lut":
                    return 2;
                case "mar":
                    return 3;
                case "kwi":
                    return 4;
                case "maj":
                    return 5;
                case "cze":
                    return 6;
                case "lip":
                    return 7;
                case "sie":
                    return 8;
                case "wrz":
                    return 9;
                case "paź":
                    return 10;
                case "lis":
                    return 11;
                case "gru":
                    return 12;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
