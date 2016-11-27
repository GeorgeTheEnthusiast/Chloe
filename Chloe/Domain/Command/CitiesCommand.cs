using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using Chloe.Domain.Query;
using ChloeDto = Chloe.Dto;
using ChloeDomain = Chloe.Domain.Dto;

namespace Chloe.Domain.Command
{
    public class CitiesCommand : ICitiesCommand
    {
        private readonly ICityConverter _cityConverter;

        public CitiesCommand(ICityConverter cityConverter)
        {
            if (cityConverter == null) throw new ArgumentNullException("cityConverter");

            _cityConverter = cityConverter;
        }

        public ChloeDto.City Merge(ChloeDto.City city)
        {
            ChloeDto.City result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                ChloeDomain.Cities domainCities = _cityConverter.Convert(city);

                var existedCity = ChloeEntities.Cities
                    .Where(x => x.Name.Trim().ToUpper() == city.Name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedCity != null)
                {
                    if (string.IsNullOrEmpty(existedCity.Alias)
                        && !string.IsNullOrEmpty(city.Alias))
                    {
                        existedCity.Alias = city.Alias;
                        ChloeEntities.SaveChanges();
                    }

                    result = _cityConverter.Convert(existedCity);
                    return result;
                }

                ChloeEntities.Cities.Add(domainCities);
                ChloeEntities.SaveChanges();
            }
            
            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var existedCity = ChloeEntities.Cities
                    .Where(x => x.Name.Trim().ToUpper() == city.Name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();
                
                result = _cityConverter.Convert(existedCity);

                return result;
            }
        }
    }
}
