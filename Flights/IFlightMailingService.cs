using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights
{
    public interface IFlightMailingService
    {
        void SendResults(DateTime fromDate);
    }
}
