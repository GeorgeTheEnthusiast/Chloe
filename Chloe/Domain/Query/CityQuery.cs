using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;
using Chloe.Dto.Enums;
using Chloe.Exceptions;

namespace Chloe.Domain.Query
{
    public class CityQuery : ICityQuery
    {
        private readonly ICityConverter _cityConverter;

        public CityQuery(ICityConverter cityConverter)
        {
            if (cityConverter == null) throw new ArgumentNullException("cityConverter");

            _cityConverter = cityConverter;
        }

        public ChloeDto.City GetCityByName(string name)
        {
            ChloeDto.City result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var cityDomain = ChloeEntities.Cities
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
