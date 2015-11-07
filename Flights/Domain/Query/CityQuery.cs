using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Dto;

namespace Flights.Domain.Query
{
    public class CityQuery : ICityQuery
    {
        public List<Cities> GetAllCities()
        {
            List<Cities> result;

            using (var flightDataModel = new FlightsEntities())
            {
                result = flightDataModel.Cities.ToList();
            }

            return result;
        }
    }
}
