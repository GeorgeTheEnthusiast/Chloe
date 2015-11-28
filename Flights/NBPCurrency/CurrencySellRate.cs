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

        public CurrencySellRate(IXmlParser xmlParser)
        {
            if (xmlParser == null) throw new ArgumentNullException("xmlParser");

            _xmlParser = xmlParser;
        }

        public decimal GetSellRate(Currency currency)
        {
            if (currency.Name == "zł")
                return 1;
            
            var tabelaKursow = _xmlParser.Parse();
            string currencyCode = string.Empty;

            switch (currency.Name)
            {
                case "Nkr":
                case "kr":
                    currencyCode = "NOK";
                    break;
                case "£":
                    currencyCode = "GBP";
                    break;
                case "€":
                    currencyCode = "EUR";
                    break;
                default:
                    throw new NotSupportedException("Ta waluta nie jest obsługiwana!");
            }

            var nok = tabelaKursow.pozycja
                        .FirstOrDefault(x => x.kod_waluty.Trim() == currencyCode);

            if (nok == null)
                throw new NotSupportedException("Ta waluta nie znajduje się w danych od NBP!");

            nok.kurs_sredni = nok.kurs_sredni.Replace(',', '.');
            nok.przelicznik = nok.przelicznik.Replace(',', '.');

            decimal middle = decimal.Parse(nok.kurs_sredni, CultureInfo.InvariantCulture);
            decimal conversion = decimal.Parse(nok.przelicznik, CultureInfo.InvariantCulture);

            return middle / conversion;
        }
    }
}
