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
    public class CarrierCommand : ICarrierCommand
    {
        private readonly ICarrierConverter _carrierConverter;

        public CarrierCommand(ICarrierConverter carrierConverter)
        {
            if (carrierConverter == null) throw new ArgumentNullException("carrierConverter");

            _carrierConverter = carrierConverter;
        }

        public ChloeDto.Carrier Merge(string name)
        {
            ChloeDto.Carrier result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var existedCarrier = ChloeEntities.Carriers
                    .Where(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedCarrier != null)
                {
                    result = _carrierConverter.Convert(existedCarrier);

                    return result;
                }
                else
                {
                    ChloeEntities.Carriers.Add(new ChloeDomain.Carriers()
                    {
                      Name  = name
                    });
                }
                
                ChloeEntities.SaveChanges();
            }

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var existedCarrier = ChloeEntities.Carriers
                    .Where(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper())
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                result = _carrierConverter.Convert(existedCarrier);
            }

            return result;
        }
    }
}
