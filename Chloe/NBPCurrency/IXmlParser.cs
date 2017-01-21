using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.NBPCurrency
{
    public interface IXmlParser
    {
        tabela_kursow Parse();
    }
}
