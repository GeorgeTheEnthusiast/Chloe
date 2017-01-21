using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Dto
{
    public class NotificationReceiverGroup
    {
        public NotificationReceiver NotificationReceiver { get; set; }

        public ReceiverGroup ReceiverGroup { get; set; }
    }
}
