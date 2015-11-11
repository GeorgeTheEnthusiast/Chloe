using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Query
{
    public class CountryQuery : ICountryQuery
    {
        private readonly ICountriesConverter _countriesConverter;

        public CountryQuery(ICountriesConverter countriesConverter)
        {
            if (countriesConverter == null) throw new ArgumentNullException("countriesConverter");

            _countriesConverter = countriesConverter;
        }

        public IEnumerable<FlightsDto.Country> GetAllCountries()
        {
            IEnumerable<FlightsDto.Country> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var countriesDomain = flightDataModel.Countries.ToList();
                result = _countriesConverter.Convert(countriesDomain);
            }

            return result;
        }
    }
}
