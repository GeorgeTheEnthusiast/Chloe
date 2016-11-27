using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Converters
{
    public interface IRyanAirDateConverter
    {
        DateTime Convert(DateTime dateToMergeWith, string input);
    }
}
