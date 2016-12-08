using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDto = Flights.Dto;
using FlightsDomain = Flights.Domain.Dto;

namespace Flights.Domain.Command
{
    public class NetCommand : INetCommand
    {
        private readonly INetConverter _netConverter;

        public NetCommand(INetConverter netConverter)
        {
            if (netConverter == null) throw new ArgumentNullException("netConverter");

            _netConverter = netConverter;
        }

        public FlightsDto.Net Merge(FlightsDto.Net net)
        {
            FlightsDto.Net result;

            using (FlightsDomain.FlightsEntities flightsEntities = new FlightsDomain.FlightsEntities())
            {
                FlightsDomain.Net domainNet = _netConverter.Convert(net);

                var existedNet = flightsEntities.Net
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
                    domainNet = flightsEntities.Net.Add(domainNet);
                }
                
                result = _netConverter.Convert(domainNet);
                flightsEntities.SaveChanges();
            }

            return result;
        }
    }
}
