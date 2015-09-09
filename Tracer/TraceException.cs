using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing
{
    /// <summary>
    /// Tracing internal exception.
    /// </summary>
    public class TraceException : System.Exception
    {
        public TraceException() : base() { }
        public TraceException(string message) : base(message) { }
        public TraceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
