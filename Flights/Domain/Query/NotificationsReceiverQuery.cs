using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Query
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

        public IEnumerable<FlightsDto.NotificationReceiver> GetAllNotificationsReceivers()
        {
            IEnumerable<FlightsDto.NotificationReceiver> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var notificationReceiversDomain = flightDataModel.NotificationsReceivers.ToList();
                result = _notificationReceiversConverter.Convert(notificationReceiversDomain);
            }

            return result;
        }
    }
}
