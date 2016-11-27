using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Converters
{
    public interface INorwegianDateConverter
    {
        DateTime Convert(string input);
    }
}
