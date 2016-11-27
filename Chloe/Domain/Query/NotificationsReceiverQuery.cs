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
    public class NotificationsReceiverQuery : INotificationsReceiverQuery
    {
        private readonly INotificationReceiversConverter _notificationReceiversConverter;

        public NotificationsReceiverQuery(INotificationReceiversConverter notificationReceiversConverter)
        {
            if (notificationReceiversConverter == null)
                throw new ArgumentNullException("notificationReceiversConverter");

            _notificationReceiversConverter = notificationReceiversConverter;
        }

        public IEnumerable<ChloeDto.NotificationReceiver> GetAllNotificationsReceivers()
        {
            IEnumerable<ChloeDto.NotificationReceiver> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var notificationReceiversDomain = flightDataModel.NotificationReceivers.ToList();
                result = _notificationReceiversConverter.Convert(notificationReceiversDomain);
            }

            return result;
        }
    }
}
