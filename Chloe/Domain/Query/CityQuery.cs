﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;
using Flights.Dto.Enums;
using Flights.Exceptions;

namespace Flights.Domain.Query
{
    public class CityQuery : ICityQuery
    {
        private readonly ICityConverter _cityConverter;

        public CityQuery(ICityConverter cityConverter)
        {
            if (cityConverter == null) throw new ArgumentNullException("cityConverter");

            _cityConverter = cityConverter;
        }

        public FlightsDto.City GetCityByName(string name)
        {
            FlightsDto.City result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                var cityDomain = flightsEntities.Cities
                                        .DefaultIfEmpty(null)
                                        .FirstOrDefault(x => x.Name.Trim() == name);

                if (cityDomain == null)
                    throw new EntityNotFoundException();

                result = _cityConverter.Convert(cityDomain);
            }

            return result;
        }
    }
}
