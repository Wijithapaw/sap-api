using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Exceptions
{
    public class SapException: ApplicationException
    {
        public SapException(): base() { }
        public SapException(string message) : base(message) { }
        public SapException(string message, Exception innerException) : base(message, innerException) { }
    }
}
