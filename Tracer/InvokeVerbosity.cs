using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing
{
    /// <summary>
    /// Different Log levels supported
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        /// Logs verbose output.
        /// </summary>
        Verbose = 1,
        /// <summary>
        /// Logs basic information.NOTE: conside Info
        /// </summary>
        Information = 2,
        /// <summary>
        /// Logs a warning.
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Logs an error.
        /// </summary>
        Error = 4,
        /// <summary>
        /// Logs a fatal incident.
        /// </summary>
        Fatal = 5
    }

    /// <summary>
    /// InvokeVerbosity enumerates supported 'method call' raised events.
    /// By default, each call will raise OnEnter, OnLeave events before/after
    /// calling the method, respectively. When exception occurs, OnException event is
    /// raised. User can opt to only raise desired event(s).
    /// </summary>
    public enum InvokeVerbosity
    {
        /// <summary>
        /// Invoke will not raise event. Very rarely used, but is available.
        /// </summary>
        None,
        /// <summary>
        /// OnEnter event will be raised before calling the method.
        /// </summary>
        OnEnter = 1,
        /// <summary>
        /// OnLeave event will be raised after calling the method.
        /// </summary>
        OnLeave = 2,
        /// <summary>
        /// OnException event will be raised when an exception is thrown during a method call.
        /// </summary>
        OnException = 4,
        /// <summary>
        /// (Trace logging specific) Don't log the method parameter values.
        /// </summary>
        HideParameterValues = 128,
        /// <summary>
        /// Don't measure method invoke run time.
        /// By default method run time is measured by getting delta between
        /// start and end times before and after calling the method.
        /// </summary>
        NoRunTimeMeasurement = 256,

        /// <summary>
        /// Verbose will raise all events, OnEnter, OnLeave and OnException
        /// as needed.
        /// </summary>
        Verbose = OnEnter | OnLeave | OnException,
        /// <summary>
        /// Default is same as Verbose.
        /// </summary>
        Default = Verbose
    }
}
