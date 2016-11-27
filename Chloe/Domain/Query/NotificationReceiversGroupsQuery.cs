using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Domain.Query
{
    public class NotificationReceiversGroupsQuery : INotificationReceiversGroupsQuery
    {
        private readonly INotificationReceiversGroupsConverter _notificationReceiversGroupsConverter;

        public NotificationReceiversGroupsQuery(INotificationReceiversGroupsConverter notificationReceiversGroupsConverter)
        {
            if (notificationReceiversGroupsConverter == null)
                throw new ArgumentNullException("notificationReceiversGroupsConverter");

            _notificationReceiversGroupsConverter = notificationReceiversGroupsConverter;
        }

        public IEnumerable<ChloeDto.NotificationReceiverGroup> GetNotificationReceiversGroupsByReceiverGroup(ChloeDto.ReceiverGroup receiverGroup)
        {
            IEnumerable<ChloeDto.NotificationReceiverGroup> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var notificationReceiversDomain =
                    flightDataModel.NotificationReceiversGroups.Where(x => x.ReceiverGroups_Id == receiverGroup.Id);
                result = _notificationReceiversGroupsConverter.Convert(notificationReceiversDomain);
            }

            return result;
        }

        public IEnumerable<ChloeDto.NotificationReceiverGroup> GetAllNotificationReceiversGroups()
        {
            IEnumerable<ChloeDto.NotificationReceiverGroup> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var notificationReceiversDomain = flightDataModel.NotificationReceiversGroups;
                result = _notificationReceiversGroupsConverter.Convert(notificationReceiversDomain);
            }

            return result;
        }
    }
}
