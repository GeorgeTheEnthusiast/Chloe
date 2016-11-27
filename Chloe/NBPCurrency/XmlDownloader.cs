using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.NBPCurrency
{
    public class XmlDownloader : IXmlDownloader
    {
        private string xml = "LastA.xml";

        public void Download()
        {
            File.Delete(xml);

            using (var client = new WebClient())
            {
                client.DownloadFile("http://www.nbp.pl/kursy/xml/LastA.xml", xml);
            }
        }
    }
}
