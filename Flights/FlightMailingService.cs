using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Query;
using System.Net.Mail;

namespace Flights
{
    public class FlightMailingService : IFlightMailingService
    {
        private readonly IFlightsQuery _flightsQuery;

        public FlightMailingService(IFlightsQuery flightsQuery)
        {
            if (flightsQuery == null) throw new ArgumentNullException("flightsQuery");

            _flightsQuery = flightsQuery;
        }

        public void SendResults(DateTime fromDate)
        {
            var flightsToSend = _flightsQuery.GetFlightsBySearchDate(fromDate);

            MailAddress mailAddressFrom = new MailAddress("info@loty.gk", "Wyszukiwarka lotów GK");
            MailAddress mailAddressTo = new MailAddress("warmemotions@gmail.com");
            MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);
            mailMessage.Body = "Test";
            mailMessage.Subject = "Najtańsze loty na dzień " + DateTime.Now;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Send(mailMessage);
        }
    }
}
