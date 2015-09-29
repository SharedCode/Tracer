// Tracer v1.0
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary;
using Tracing.InvokeEngine;
using Tracing;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tracing.MSEnterpriseLogging
{
    /// <summary>
    /// Enterprise TraceScope.
    /// </summary>
    public class TraceScope : TraceScope<EnterpriseTracer>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        /// <param name="category"></param>
        public TraceScope(string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            string[] category = null)
            : base(null, funcFootprint, verbosity, null, category)
        { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tracer"></param>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        /// <param name="category"></param>
        public TraceScope(EnterpriseTracer tracer,
            string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string[] category = null)
            : base(tracer, funcFootprint, verbosity, rethrowOnError, category)
        { }
    }

    /// <summary>
    /// Enterprise Tracer is the Tracer's adaptation to 
    /// Microsoft Enterprise Logging Framework.
    /// NOTE: this is just a sample reference implementation.
    /// Change it based on your needs, tastes and req'ts.
    /// </summary>
    public class EnterpriseTracer : Tracing.Tracer
    {
        public EnterpriseTracer() : this(null, null) { }
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EnterpriseTracer(string[] category = null, ResultEvaluatorDelegate resultEvaluator = null,
            bool hookDefaultEventHandler = true) : base(category, resultEvaluator)
        {
            if (hookDefaultEventHandler)
            {
                OnLog += Log;
                OnLogException += LogException;
            }
        }

        private void LogException(System.Exception exc, string[] category, string message)
        {
            try
            {
                Log(LogLevels.Error, category, string.Format(MessageStrings.MessageWithDetailsMessageTemplate, message, exc.ToString()));
            }
            catch (System.Exception logExc)
            {
                throw new TraceException(Format(MessageStrings.FailedLoggingMessageTemplate, message), logExc);
            }
        }

        private void Log(LogLevels logLevel, string[] category, string message)
        {
            try
            {
                // Populate LogEntry w/ log message.
                LogEntry logEntry = new LogEntry();
                logEntry.Message = message;
                if (category != null)
                {
                    foreach (var c in category)
                        logEntry.Categories.Add(c);
                }
                logEntry.Priority = 1;
                //logEntry.EventId = (int)
                //    (logLevel >= LogLevels.Warning ? 10001 : 10002);
                logEntry.Severity = Tracer.Convert(logLevel);
                //if (extendedProps != null)
                //    logEntry.ExtendedProperties = extendedProps;
                Logger.Write(logEntry);
            }
            catch (System.Exception exc)
            {
                throw new TraceException(Tracer.Format(MessageStrings.FailedLoggingMessageTemplate, message), exc);
            }
        }
    }
}
