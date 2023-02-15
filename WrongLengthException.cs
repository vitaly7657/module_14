using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace module_14
{
    internal class WrongLengthException : Exception
    {
        public string ExceptionMessage { get; set; }
        public WrongLengthException(string message)
        {
            this.ExceptionMessage = message;
        }
    }
}
