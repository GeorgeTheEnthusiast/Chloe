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
    public class NotificationReceiversGroupsQuery : INotificationReceiversGroupsQuery
    {
        private readonly INotificationReceiversGroupsConverter _notificationReceiversGroupsConverter;

        public NotificationReceiversGroupsQuery(INotificationReceiversGroupsConverter notificationReceiversGroupsConverter)
        {
            if (notificationReceiversGroupsConverter == null)
                throw new ArgumentNullException("notificationReceiversGroupsConverter");

            _notificationReceiversGroupsConverter = notificationReceiversGroupsConverter;
        }

        public IEnumerable<FlightsDto.NotificationReceiverGroup> GetNotificationReceiversGroupsByReceiverGroup(FlightsDto.ReceiverGroup receiverGroup)
        {
            IEnumerable<FlightsDto.NotificationReceiverGroup> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var notificationReceiversDomain =
                    flightDataModel.NotificationReceiversGroups.Where(x => x.ReceiverGroups_Id == receiverGroup.Id);
                result = _notificationReceiversGroupsConverter.Convert(notificationReceiversDomain);
            }

            return result;
        }

        public IEnumerable<FlightsDto.NotificationReceiverGroup> GetAllNotificationReceiversGroups()
        {
            IEnumerable<FlightsDto.NotificationReceiverGroup> result;

            using (var flightDataModel = new FlightsDomain.FlightsEntities())
            {
                var notificationReceiversDomain = flightDataModel.NotificationReceiversGroups;
                result = _notificationReceiversGroupsConverter.Convert(notificationReceiversDomain);
            }

            return result;
        }
    }
}
