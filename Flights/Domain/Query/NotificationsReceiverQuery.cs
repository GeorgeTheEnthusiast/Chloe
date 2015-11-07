using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Dto;

namespace Flights.Domain.Query
{
    public class NotificationsReceiverQuery : INotificationsReceiverQuery
    {
        public List<NotificationsReceivers> GetAllNotificationsReceivers()
        {
            List<NotificationsReceivers> result;

            using (var flightDataModel = new FlightsEntities())
            {
                result = flightDataModel.NotificationsReceivers.ToList();
            }

            return result;
        }
    }
}
