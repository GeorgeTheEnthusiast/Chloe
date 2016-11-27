using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Dto
{
    public class TimeTable
    {
        public int Id { get; set; }

        public Carrier Carrier { get; set; }

        public City CityFrom { get; set; }

        public City CityTo { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public FlightWebsite FlightWebsite { get; set; }
    }
}
