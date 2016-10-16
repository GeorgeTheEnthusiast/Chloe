using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;

namespace Flights.NBPCurrency
{
    public class CurrencySellRate : ICurrencySellRate
    {
        private readonly IXmlParser _xmlParser;

        private readonly List<string> plnNames = new List<string>() { "zł", "zl", "pln" }; 

        public CurrencySellRate(IXmlParser xmlParser)
        {
            if (xmlParser == null) throw new ArgumentNullException("xmlParser");

            _xmlParser = xmlParser;
        }

        public decimal GetSellRate(Currency currency)
        {
            if (plnNames.Contains(currency.Name.ToLower()))
                return 1;
            
            var tabelaKursow = _xmlParser.Parse();
            string currencyCode = string.Empty;

            switch (currency.Name)
            {
                case "nok":
                case "NOK":
                case "Nkr":
                case "kr":
                    currencyCode = "NOK";
                    break;
                case "gbp":
                case "GBP":
                case "£":
                    currencyCode = "GBP";
                    break;
                case "eur":
                case "EUR":
                case "€":
                    currencyCode = "EUR";
                    break;
                default:
                    throw new NotSupportedException(string.Format("This currency [{0}] is not supported!", currency.Name));
            }

            var nok = tabelaKursow.pozycja
                        .FirstOrDefault(x => x.kod_waluty.Trim() == currencyCode);

            if (nok == null)
                throw new NotSupportedException(string.Format("This currency [{0}] is not present in the NBP data!", currencyCode));

            nok.kurs_sredni = nok.kurs_sredni.Replace(',', '.');
            nok.przelicznik = nok.przelicznik.Replace(',', '.');

            decimal middle = decimal.Parse(nok.kurs_sredni, CultureInfo.InvariantCulture);
            decimal conversion = decimal.Parse(nok.przelicznik, CultureInfo.InvariantCulture);

            return middle / conversion;
        }
    }
}
