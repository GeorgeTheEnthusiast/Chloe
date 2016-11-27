using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDto = Chloe.Dto;
using ChloeDomain = Chloe.Domain.Dto;

namespace Chloe.Domain.Command
{
    public class NetCommand : INetCommand
    {
        private readonly INetConverter _netConverter;

        public NetCommand(INetConverter netConverter)
        {
            if (netConverter == null) throw new ArgumentNullException("netConverter");

            _netConverter = netConverter;
        }

        public ChloeDto.Net Merge(ChloeDto.Net net)
        {
            ChloeDto.Net result;

            using (ChloeDomain.ChloeEntities ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                ChloeDomain.Net domainNet = _netConverter.Convert(net);

                var existedNet = ChloeEntities.Net
                    .Where(x => x.Carrier_Id == net.Carrier.Id
                    && x.CityFrom_Id == net.CityFrom.Id
                    && x.CityTo_Id == net.CityTo.Id)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

                if (existedNet != null)
                {
                    domainNet = existedNet;
                }
                else
                {
                    domainNet = ChloeEntities.Net.Add(domainNet);
                }
                
                result = _netConverter.Convert(domainNet);
                ChloeEntities.SaveChanges();
            }

            return result;
        }
    }
}
