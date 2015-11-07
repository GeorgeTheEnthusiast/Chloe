using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Dto
{
    public class Flight
    {
        public int Id { get; set; }

        public SearchCriteria SearchCriteria { get; set; }

        public DateTime DepartureTime { get; set; }

        public Currency Currency { get; set; }

        public int Price { get; set; }

        public DateTime SearchDate { get; set; }

        public string SearchValidationText { get; set; }

        public Carrier Carrier { get; set; }
    }
}
