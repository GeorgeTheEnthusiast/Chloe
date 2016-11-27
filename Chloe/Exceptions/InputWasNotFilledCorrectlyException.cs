using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Exceptions
{
    public class InputWasNotFilledCorrectlyException : Exception
    {
        public string Name { get; set; }
    }
}
