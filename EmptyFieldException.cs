using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace module_14
{
    internal class EmptyFieldException : Exception
    {
        public string ExceptionMessage { get; set; }
        public EmptyFieldException(string message)
        {
            this.ExceptionMessage = message;
        }

    }
}
