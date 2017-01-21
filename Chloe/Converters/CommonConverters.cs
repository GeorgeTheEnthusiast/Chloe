using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Converters
{
    public class CommonConverters : ICommonConverters
    {
        public string ConvertBoolToYesNo(bool input)
        {
            if (input)
                return "Tak";
            else
                return "Nie";
        }
    }
}
