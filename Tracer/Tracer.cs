// Tracer v1.0
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Tracing.InvokeEngine;

namespace Tracing
{
    /// <summary>
    /// On Log event delegate.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public delegate void OnLogDelegate(LogLevels logLevel, string message);

    /// <summary>
    /// Invoke Wrapper
    /// </summary>
    public partial class Tracer
    {
        /// <summary>
        /// true will generate before/after method call (information) logging.
        /// </summary>
        override public bool InvokeTracing
        {
            get
            {
                return base.InvokeTracing;
            }
            set
            {
                base.InvokeTracing = value;
                if (value)
                    // upgrade log level from Verbose to Information as 
                    // user "special request" to generate trace logging.
                    _tracingLogLevel = LogLevels.Information;
            }
        }

        internal protected override bool OnExceptionHandler(Exception exc, string functionInfo, InvokeVerbosity verbosity = InvokeVerbosity.Verbose)
        {
            if (base.OnExceptionHandler(exc, functionInfo, verbosity))
                return true;
            // only log exception if verbosity flag for it is set.
            if (!IsSet(verbosity, InvokeVerbosity.OnException))
                return true;
            try
            {
                if (_onLogExceptionHandler != null)
                {
                    _onLogExceptionHandler(EventType.OnException, exc, Format("Failed calling {0}.", functionInfo));
                    return true;
                }
            }
            catch (TraceException internalExc)
            {
                OnEventExceptionHandler(EventType.OnException, internalExc, functionInfo);
            }
            return false;
        }

        //public string GetRunTime(string functionFootPrint, DateTime startTime)
        //{
        //    return string.Format("{0}, run time(secs): {1}", functionFootPrint, DateTime.Now.Subtract(startTime).TotalSeconds);
        //}
        //public string GetRunTime(MethodInfo method, DateTime startTime)
        //{
        //    var funcName = Format(method);
        //    return GetRunTime(funcName, startTime);
        //}

        #region Logging

        public event OnEventExceptionDelegate OnLogException
        {
            add { _onLogExceptionHandler += value; }
            remove { _onLogExceptionHandler -= value; }
        }
        private event OnEventExceptionDelegate _onLogExceptionHandler;
        public event OnLogDelegate OnLog
        {
            add { _onLogHandler += value; }
            remove { _onLogHandler -= value; }
        }
        private event OnLogDelegate _onLogHandler;

        #region Log Formatting functions
        /// <summary>
        /// Utility function for Formatting text msg and optional messageArgs.
        /// </summary>
        /// <param name="messageFormat">Text message containing formatting section.</param>
        /// <param name="messageArgs">Arguments to be rendered/formatted part of the Text message.</param>
        /// <returns>Actual formatted text that got logged.</returns>
        private static string Format(string messageFormat, params object[] messageArgs)
        {
            string data;
            if (messageFormat != null && messageArgs != null && messageArgs.Length > 0)
                data = string.Format(messageFormat, messageArgs);
            else
                data = messageFormat;
            return data;
        }
        #endregion

        /// <summary>
        /// Convert internally used LogLevel into .Net Trace Event Type.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public System.Diagnostics.TraceEventType Convert(LogLevels logLevel)
        {
            return logLevel == LogLevels.Verbose ? System.Diagnostics.TraceEventType.Verbose :
                            logLevel == LogLevels.Information ? System.Diagnostics.TraceEventType.Information :
                            logLevel == LogLevels.Warning ? System.Diagnostics.TraceEventType.Warning :
                            logLevel == LogLevels.Error ? System.Diagnostics.TraceEventType.Error :
                            System.Diagnostics.TraceEventType.Critical;
        }
        #endregion
        private LogLevels _tracingLogLevel = LogLevels.Verbose;
    }
}
