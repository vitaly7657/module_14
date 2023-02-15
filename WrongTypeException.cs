using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace module_14
{
    internal class WrongTypeException : Exception
    {
        public string ExceptionMessage { get; set; }
        public WrongTypeException(string message)
        {
            this.ExceptionMessage = message;
        }
    }
}
