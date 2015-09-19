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
    /// Enterprise Tracer is the Tracer's adaptation to 
    /// Microsoft Enterprise Logging Framework.
    /// NOTE: this is just a sample reference implementation.
    /// Change it based on your needs, tastes and req'ts.
    /// </summary>
    public class EnterpriseTracer : Tracing.SlimTracer
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
                OnLog += (LogLevels logLevel, string message) => { Log(logLevel, message); };
                OnLogException += LogException;
            }
        }

        private void LogException(System.Exception exc, string message)
        {
            try
            {
                Log(LogLevels.Error, string.Format("{0} Details: {1}", message, exc.ToString()));
            }
            catch (System.Exception logExc)
            {
                throw new TraceException(Format("Failed logging {0}.", message), logExc);
            }
        }

        private void Log(LogLevels logLevel, string message, Dictionary<string, object> extendedProps = null)
        {
            try
            {
                // Populate LogEntry w/ log message.
                LogEntry logEntry = new LogEntry();
                logEntry.Message = message;
                if (Category != null)
                {
                    foreach (var c in Category)
                        logEntry.Categories.Add(c);
                }
                logEntry.Priority = 1;
                //logEntry.EventId = (int)
                //    (logLevel >= LogLevels.Warning ? 10001 : 10002);
                logEntry.Severity = Tracer.Convert(logLevel);
                if (extendedProps != null)
                    logEntry.ExtendedProperties = extendedProps;
                Logger.Write(logEntry);
            }
            catch (System.Exception exc)
            {
                throw new TraceException(Tracer.Format("Failed logging {0}.", message), exc);
            }
        }
    }
}
