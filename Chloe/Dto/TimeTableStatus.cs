using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Dto
{
    public class TimeTableStatus
    {
        public int Id { get; set; }

        public FlightWebsite FlightWebsite { get; set; }

        public City CityFrom { get; set; }

        public City CityTo { get; set; }

        public DateTime? SearchDate { get; set; }
    }
}
