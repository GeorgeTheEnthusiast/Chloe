using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Flights.Domain.Dto;
using FlightDto = Flights.Dto;

namespace Flights.Converters
{
    public interface INotificationReceiversConverter
    {
        List<FlightDto.NotificationReceiver> Convert(List<FlightDomain.NotificationsReceivers> notificationsReceivers);
    }
}
