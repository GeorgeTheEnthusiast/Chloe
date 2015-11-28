using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flights.Domain.Query;
using System.Net.Mail;
using Flights.Converters;
using Flights.Dto;
using Flights.NBPCurrency;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;

namespace Flights
{
    public class FlightMailingService : IFlightMailingService
    {
        private readonly IFlightsQuery _flightsQuery;
        private readonly ICommonConverters _commonConverters;
        private readonly INotificationReceiversGroupsQuery _notificationReceiversGroupsQuery;
        private readonly ICurrencySellRate _currencySellRate;
        private Dictionary<string, List<string>> receiversDictionary = new Dictionary<string, List<string>>();
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public FlightMailingService(IFlightsQuery flightsQuery, 
            ICommonConverters commonConverters,
            INotificationReceiversGroupsQuery notificationReceiversGroupsQuery,
            ICurrencySellRate currencySellRate)
        {
            if (flightsQuery == null) throw new ArgumentNullException("flightsQuery");
            if (commonConverters == null) throw new ArgumentNullException("commonConverters");
            if (notificationReceiversGroupsQuery == null)
                throw new ArgumentNullException("notificationReceiversGroupsQuery");
            if (currencySellRate == null) throw new ArgumentNullException("currencySellRate");

            _flightsQuery = flightsQuery;
            _commonConverters = commonConverters;
            _notificationReceiversGroupsQuery = notificationReceiversGroupsQuery;
            _currencySellRate = currencySellRate;
        }

        public void SendResults()
        {
            var notificationReceiversGroups = _notificationReceiversGroupsQuery.GetAllNotificationReceiversGroups();

            foreach (var nrg in notificationReceiversGroups.GroupBy(x => x.ReceiverGroup))
            {
                var mailReceivers = nrg.ToList();

                if (!mailReceivers.Any())
                    continue;

                string pdfFileName = CreatePdf(nrg.Key);

                if (pdfFileName == null)
                    continue;

                foreach (var receiver in mailReceivers)
                {
                    if (receiversDictionary.ContainsKey(receiver.NotificationReceiver.Email) == false)
                        receiversDictionary[receiver.NotificationReceiver.Email] = new List<string>();

                    if (receiversDictionary[receiver.NotificationReceiver.Email].Contains(pdfFileName) == false)
                        receiversDictionary[receiver.NotificationReceiver.Email].Add(pdfFileName);
                }
            }

            SendMails();
        }

        private string CreatePdf(ReceiverGroup receiverGroup)
        {
            Document doc = new Document();
            string fileName = string.Format("cheapest_flights_{0}_{1}.pdf", receiverGroup.Name, DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
            var flightsToSend = _flightsQuery.GetFlightsByReceiverGroup(receiverGroup);

            if (!flightsToSend.Any())
                return null;

            ConvertPricesToPLN(ref flightsToSend);
            var searchGroups = flightsToSend.GroupBy(x => x.SearchCriteria.DepartureDate);
            var fontBold = FontFactory.GetFont(BaseFont.TIMES_BOLD, BaseFont.CP1257, 8, Font.BOLD);
            var fontNormal = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 8, Font.NORMAL);
            var fontHeader = FontFactory.GetFont(BaseFont.TIMES_BOLD, BaseFont.CP1257, 12, Font.BOLD);

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.Open();
                
                foreach (var group in searchGroups)
                {
                    var flightsFromCountry = flightsToSend
                        .Where(x => x.SearchCriteria.DepartureDate == group.Key)
                        .OrderBy(x => x.Price);

                    if (!flightsFromCountry.Any())
                        continue;
                    
                    doc.Add(new Phrase(string.Format("Loty blisko {0}", group.Key.Date.ToShortDateString()), fontHeader));

                    PdfPTable table = new PdfPTable(7);
                    float[] cellWidths = new float[] {32f, 32f, 28f, 14f, 18f, 10f, 28f};
                    table.SetWidths(cellWidths);

                    table.AddCell(new Phrase("Od", fontBold));
                    table.AddCell(new Phrase("Do", fontBold));
                    table.AddCell(new Phrase("Data wylotu", fontBold));
                    table.AddCell(new Phrase("Cena [zł]", fontBold));
                    table.AddCell(new Phrase("Przewoźnik", fontBold));
                    table.AddCell(new Phrase("Lot bezpośredni", fontBold));
                    table.AddCell(new Phrase("Dane z dnia", fontBold));

                    foreach (var flight in flightsFromCountry)
                    {
                        table.AddCell(new Phrase(flight.SearchCriteria.CityFrom.Name, fontNormal));
                        table.AddCell(new Phrase(flight.SearchCriteria.CityTo.Name, fontNormal));
                        table.AddCell(new Phrase(flight.DepartureTime.ToString(), fontNormal));
                        table.AddCell(new Phrase(Math.Round(flight.Price).ToString(), fontNormal));
                        table.AddCell(new Phrase(flight.Carrier.Name, fontNormal));

                        string isDirect = _commonConverters.ConvertBoolToYesNo(flight.IsDirect);
                        table.AddCell(new Phrase(isDirect, fontNormal));
                        
                        table.AddCell(new Phrase(flight.SearchDate.ToString(), fontNormal));
                    }

                    doc.Add(table);
                }
                
                doc.Close();
            }

            return fileName;
        }

        private void SendMails()
        {
            foreach (var receiver in receiversDictionary)
            {
                MailAddress mailAddressFrom = new MailAddress("flights.adam.kwiat@gmail.com", "Wyszukiwarka lotów GK");
                MailAddress mailAddressTo = new MailAddress(receiver.Key);
                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);
                mailMessage.Subject = string.Format("Najtańsze loty na dzień {0}", DateTime.Now);
                mailMessage.Body = "Loty są sortowane wg waluty, a następnie po cenie.";

                foreach (var pdf in receiver.Value)
                {
                    mailMessage.Attachments.Add(new Attachment(pdf));
                }
                
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("flights.adam.kwiat", "ToJest1KontoPocztowe");
                smtpClient.Send(mailMessage);
            }
        }

        private void ConvertPricesToPLN(ref IEnumerable<Flight> flights)
        {
            foreach (var flight in flights)
            {
                decimal sellRate = _currencySellRate.GetSellRate(flight.Currency);
                flight.Price = flight.Price * sellRate;
            }
        } 
    }
}
