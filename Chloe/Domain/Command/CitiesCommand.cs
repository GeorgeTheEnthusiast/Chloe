using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Domain.Query;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Domain.Command
{
    public class CitiesCommand : ICitiesCommand
    {
        private readonly ICityConverter _cityConverter;

        public CitiesCommand(ICityConverter cityConverter)
        {
            if (cityConverter == null) throw new ArgumentNullException("cityConverter");

            _cityConverter = cityConverter;
        }

        public FlightsDto.City Merge(FlightsDto.City city)
        {
            FlightsDto.City result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                FlightsDomain.Cities domainCities = _cityConverter.Convert(city);

                var existedCity = flightsEntities.Cities
                    .Where(x => x.Name.Trim().ToUpper() == city.Name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedCity != null)
                {
                    if (string.IsNullOrEmpty(existedCity.Alias)
                        && !string.IsNullOrEmpty(city.Alias))
                    {
                        existedCity.Alias = city.Alias;
                        flightsEntities.SaveChanges();
                    }

                    result = _cityConverter.Convert(existedCity);
                    return result;
                }

                flightsEntities.Cities.Add(domainCities);
                flightsEntities.SaveChanges();
            }
            
            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var existedCity = flightsEntities.Cities
                    .Where(x => x.Name.Trim().ToUpper() == city.Name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();
                
                result = _cityConverter.Convert(existedCity);

                return result;
            }
        }
    }
}
