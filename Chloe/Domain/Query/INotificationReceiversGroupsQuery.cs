using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Domain.Query
{
    public interface INotificationReceiversGroupsQuery
    {
        IEnumerable<ChloeDto.NotificationReceiverGroup> GetNotificationReceiversGroupsByReceiverGroup(ChloeDto.ReceiverGroup receiverGroup);

        IEnumerable<ChloeDto.NotificationReceiverGroup> GetAllNotificationReceiversGroups();
    }
}
