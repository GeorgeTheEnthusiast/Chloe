using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chloe.NBPCurrency
{
    public class XmlParser : IXmlParser
    {
        string xml = "LastA.xml";

        public tabela_kursow Parse()
        {
            tabela_kursow tabelaKursow = new tabela_kursow();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(tabela_kursow));

            using (FileStream fs = new FileStream(xml, FileMode.Open))
            {
                tabelaKursow = (tabela_kursow)xmlSerializer.Deserialize(fs);
            }

            return tabelaKursow;
        }
    }
}
