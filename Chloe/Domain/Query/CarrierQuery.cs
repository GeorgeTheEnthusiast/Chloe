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
    public class CarrierQuery : ICarrierQuery
    {
        private readonly ICarrierConverter _carrierConverter;

        public CarrierQuery(ICarrierConverter carrierConverter)
        {
            if (carrierConverter == null) throw new ArgumentNullException("carrierConverter");

            _carrierConverter = carrierConverter;
        }

        public ChloeDto.Carrier GetCarrierByName(string name)
        {
            ChloeDto.Carrier result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var carrierDomain = ChloeEntities.Carriers
                                        .DefaultIfEmpty(null)
                                        .FirstOrDefault(x => x.Name.Trim() == name.Trim());

                if (carrierDomain == null)
                    throw new EntityNotFoundException();

                result = _carrierConverter.Convert(carrierDomain);
            }

            return result;
        }
    }
}
