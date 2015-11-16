using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public class NotificationReceiversConverter : INotificationReceiversConverter
    {
        public NotificationReceiversConverter()
        {
            Mapper.CreateMap<FlightsDomain.NotificationReceivers, FlightsDto.NotificationReceiver>()
                .ForMember(x => x.Email, expression => expression.ResolveUsing(result => result.Email.Trim()));
        }

        public IEnumerable<FlightsDto.NotificationReceiver> Convert(IEnumerable<FlightsDomain.NotificationReceivers> notificationsReceivers)
        {
            return Mapper.Map<IEnumerable<FlightsDto.NotificationReceiver>>(notificationsReceivers);
        }
    }
}
