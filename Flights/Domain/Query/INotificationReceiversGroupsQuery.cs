using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Query
{
    public interface INotificationReceiversGroupsQuery
    {
        IEnumerable<FlightsDto.NotificationReceiverGroup> GetNotificationReceiversGroupsByReceiverGroup(FlightsDto.ReceiverGroup receiverGroup);

        IEnumerable<FlightsDto.NotificationReceiverGroup> GetAllNotificationReceiversGroups();
    }
}
