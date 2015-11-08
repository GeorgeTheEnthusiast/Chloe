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
            Mapper.CreateMap<FlightsDomain.NotificationsReceivers, FlightsDto.NotificationReceiver>()
                .ForMember(x => x.Email, expression => expression.ResolveUsing(result => result.Email.Trim()));
        }

        public List<FlightsDto.NotificationReceiver> Convert(List<FlightsDomain.NotificationsReceivers> notificationsReceivers)
        {
            return Mapper.Map<List<FlightsDto.NotificationReceiver>>(notificationsReceivers);
        }
    }
}
