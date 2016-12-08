using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Flights.Domain.Dto;
using FlightDto = Flights.Dto;

namespace Flights.Converters
{
    public interface INotificationReceiversGroupsConverter
    {
        IEnumerable<FlightDto.NotificationReceiverGroup> Convert(IEnumerable<FlightDomain.NotificationReceiversGroups> input);
    }
}
