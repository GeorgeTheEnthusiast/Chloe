using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Converters
{
    public interface INorwegianDateConverter
    {
        DateTime Convert(string norwegianPrintedDate);
    }
}
