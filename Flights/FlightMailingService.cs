using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Query;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Flights
{
    public class FlightMailingService : IFlightMailingService
    {
        private readonly IFlightsQuery _flightsQuery;
        private readonly INotificationsReceiverQuery _notificationsReceiverQuery;

        public FlightMailingService(IFlightsQuery flightsQuery, INotificationsReceiverQuery notificationsReceiverQuery)
        {
            if (flightsQuery == null) throw new ArgumentNullException("flightsQuery");
            if (notificationsReceiverQuery == null) throw new ArgumentNullException("notificationsReceiverQuery");

            _flightsQuery = flightsQuery;
            _notificationsReceiverQuery = notificationsReceiverQuery;
        }

        public void SendResults(DateTime fromDate)
        {
            string pdfFileName = CreatePdf(fromDate);
            var mailReceivers = _notificationsReceiverQuery.GetAllNotificationsReceivers();

            foreach (var receiver in mailReceivers)
            {
                MailAddress mailAddressFrom = new MailAddress("flights.adam.kwiat@gmail.com", "Wyszukiwarka lotów GK");
                MailAddress mailAddressTo = new MailAddress(receiver.Email);
                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);
                mailMessage.Subject = "Najtańsze loty na dzień " + DateTime.Now;
                mailMessage.Attachments.Add(new Attachment(pdfFileName));
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("flights.adam.kwiat", "ToJest1KontoPocztowe");
                smtpClient.Send(mailMessage);
            }
        }

        private string CreatePdf(DateTime fromDate)
        {
            Document doc = new Document();
            string fileName = string.Format("cheapest_flights_{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
            var flightsToSend = _flightsQuery.GetFlightsBySearchDate(fromDate).OrderBy(x => x.Price);
            var fontBold = FontFactory.GetFont(BaseFont.TIMES_BOLD, BaseFont.CP1257, 8, Font.BOLD);
            var fontNormal = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 8, Font.NORMAL);

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();
                PdfPTable table = new PdfPTable(7);

                table.AddCell(new Phrase("Od", fontBold));
                table.AddCell(new Phrase("Do", fontBold));
                table.AddCell(new Phrase("Data wylotu", fontBold));
                table.AddCell(new Phrase("Cena", fontBold));
                table.AddCell(new Phrase("Waluta", fontBold));
                table.AddCell(new Phrase("Przewoźnik", fontBold));
                table.AddCell(new Phrase("Dane z dnia", fontBold));

                foreach (var flight in flightsToSend)
                {
                    table.AddCell(new Phrase(flight.SearchCriteria.CityFrom.Name, fontNormal));
                    table.AddCell(new Phrase(flight.SearchCriteria.CityTo.Name, fontNormal));
                    table.AddCell(new Phrase(flight.DepartureTime.ToShortDateString(), fontNormal));
                    table.AddCell(new Phrase(flight.Price.ToString(), fontNormal));
                    table.AddCell(new Phrase(flight.Currency.Name, fontNormal));
                    table.AddCell(new Phrase(flight.SearchCriteria.Carrier.Name, fontNormal));
                    table.AddCell(new Phrase(flight.SearchDate.ToString(), fontNormal));
                }
                
                doc.Add(table);
                doc.Close();
            }

            return fileName;
        }


    }
}
