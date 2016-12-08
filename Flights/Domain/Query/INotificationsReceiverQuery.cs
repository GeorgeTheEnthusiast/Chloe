using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FLightsDto = Flights.Dto;

namespace Flights.Domain.Query
{
    public interface INotificationsReceiverQuery
    {
        IEnumerable<FLightsDto.NotificationReceiver> GetAllNotificationsReceivers();
    }
}
