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
    /// Result action type enumeration can be Pass, Fail or None.
    /// Interpret ResultActionType in your OnLeave event handler
    /// to drive what to do, e.g. - log pass message if Result
    /// passed, fail message if Result failed or don't do anything
    /// (skip logging on OnLeave event) if None.
    /// Default may log Trace msg like "Leaving method...".
    /// </summary>
    public enum ResultActionType
    {
        Default,
        Pass,
        Fail,
        None
    }
    /// <summary>
    /// Invoked method result evaluator delegate.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="runTime"></param>
    /// <param name="customMessage"></param>
    /// <returns>true typically means pass or successful.
    /// Use-case example: team up this return value with your custom OnLeave event
    /// handler to cook up some logic that logs pass or fail message based on
    /// return of this evaluator function.
    /// </returns>
    public delegate ResultActionType ResultEvaluatorDelegate(object result, TimeSpan? runTime, out string customMessage);

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

        /// <summary>
        /// Default Category.
        /// </summary>
        public static string DefaultCategory = "General";
        /// <summary>
        /// Log Category.
        /// </summary>
        public string[] Category { get; set; }

        /// <summary>
        /// OnLeave event is raised after calling the submitted function for invoke.
        /// </summary>
        public event OnLeaveDelegate OnLeave
        {
            add { _onLeaveHandler += value; }
            remove { _onLeaveHandler -= value; }
        }
        private event OnLeaveDelegate _onLeaveHandler;

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
                    _onLogExceptionHandler(exc, Format("Failed calling {0}.", functionInfo));
                    return true;
                }
            }
            catch (TraceException internalExc)
            {
                OnEventExceptionHandler(EventType.OnException, internalExc, functionInfo);
            }
            return false;
        }
        #region Logging

        public event OnExceptionDelegate OnLogException
        {
            add { _onLogExceptionHandler += value; }
            remove { _onLogExceptionHandler -= value; }
        }
        private event OnExceptionDelegate _onLogExceptionHandler;
        public event OnLogDelegate OnLog
        {
            add { _onLogHandler += value; }
            remove { _onLogHandler -= value; }
        }
        private event OnLogDelegate _onLogHandler;

        /// <summary>
        /// Result Evaluator.
        /// </summary>
        public ResultEvaluatorDelegate ResultEvaluator { get; set; }

        /// <summary>
        /// Convert internally used LogLevel into .Net Trace Event Type.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        static public System.Diagnostics.TraceEventType Convert(LogLevels logLevel)
        {
            return logLevel == LogLevels.Verbose ? System.Diagnostics.TraceEventType.Verbose :
                            logLevel == LogLevels.Information ? System.Diagnostics.TraceEventType.Information :
                            logLevel == LogLevels.Warning ? System.Diagnostics.TraceEventType.Warning :
                            logLevel == LogLevels.Error ? System.Diagnostics.TraceEventType.Error :
                            System.Diagnostics.TraceEventType.Critical;
        }
        #endregion
        private LogLevels _tracingLogLevel = LogLevels.Information;
    }
}
