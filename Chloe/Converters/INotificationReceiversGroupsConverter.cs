using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Chloe.Domain.Dto;
using FlightDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface INotificationReceiversGroupsConverter
    {
        IEnumerable<FlightDto.NotificationReceiverGroup> Convert(IEnumerable<FlightDomain.NotificationReceiversGroups> input);
    }
}
