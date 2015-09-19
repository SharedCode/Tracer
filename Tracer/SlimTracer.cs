// Tracer v1.0
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Tracing.InvokeEngine;
using Tracing;

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
    /// Slim Tracer is slimmed down version of Tracer and gives opportunity
    /// for code to process method invoke results, for example, log only based
    /// on certain confition.
    /// 
    /// Very useful scenario is that, Tracer logging is very much chatty to some
    /// content. With SlimTracer, developer can choose to only generate event log
    /// only on certain condition. For example, log after method call log only
    /// when poor performance threshold condition is reached.
    /// 
    /// This allows developer to minimize logs and generate logs only when necessary.
    /// </summary>
    public class SlimTracer : InvokeWrapperBase
    {
        public SlimTracer() : this(null) { }
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SlimTracer(string[] category = null,
            ResultEvaluatorDelegate resultEvaluator = null)
        {
            ResultEvaluator = resultEvaluator;
            // set the log Category.
            Category = new string[] { DefaultCategory };
            if (category != null)
                Category = category;
        }

        #region event handlers
        /// <summary>
        /// OnLeave event is raised after calling the submitted function for invoke.
        /// </summary>
        public event OnLeaveDelegate OnLeave
        {
            add { _onLeaveHandler += value; }
            remove { _onLeaveHandler -= value; }
        }
        private event OnLeaveDelegate _onLeaveHandler;
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
        #endregion

        /// <summary>
        /// Default Category.
        /// </summary>
        public static string DefaultCategory = "General";
        /// <summary>
        /// Log Category.
        /// </summary>
        public string[] Category { get; set; }

        /// <summary>
        /// Result Evaluator.
        /// </summary>
        public ResultEvaluatorDelegate ResultEvaluator { get; set; }

        internal override protected string HandleEmptyFootprint(string footprint)
        {
            if (footprint == string.Empty)
                return "<no function info>";
            return footprint;
        }

        protected internal override void OnEnterHandler(string funcFootprint)
        {
            if (_onLogHandler == null) return;
            try
            {
                _onLogHandler(LogLevels.Information, Format(BeforeCallingLiteral, funcFootprint));
            }
            catch (Exception exc)
            {
                throw new TraceException("Failed calling OnEnter event.", exc);
            }
        }

        protected internal override void OnLeaveHandler(object result, string funcFootprint, TimeSpan? runTime)
        {
            if (_onLeaveHandler != null)
            {
                if (ResultEvaluator != null)
                    throw new NotSupportedException("Can't specify both ResultEvaluator & OnLeave event handlers.");
                try
                {
                    _onLeaveHandler(result, funcFootprint, runTime);
                    return;
                }
                catch (Exception exc)
                {
                    throw new TraceException("Failed calling OnLeave event.", exc);
                }
            }
            if (_onLogHandler == null) return;
            var message = funcFootprint;
            try
            {
                if (runTime != null)
                    message = string.Format("{0}, run time(secs): {1}", funcFootprint, runTime.Value.TotalSeconds);
                string customMessage = "";
                if (ResultEvaluator != null)
                {
                    ResultActionType r = ResultActionType.Default;
                    try
                    {
                        ResultEvaluator(result, runTime, out customMessage);
                    }
                    catch (System.Exception exc)
                    {
                        throw new TraceException(Format("OnLeave event ResultEvaluator call failed, details: {0}.", message), exc);
                    }
                    switch (r)
                    {
                        case ResultActionType.None:
                            return;
                        case ResultActionType.Pass:
                            if (string.IsNullOrWhiteSpace(customMessage))
                                _onLogHandler(LogLevels.Information, Format("Successful call {0}.", message));
                            else
                                _onLogHandler(LogLevels.Information, Format("Successful call {0}, details: {1}.",
                                    message, customMessage));
                            return;
                        case ResultActionType.Fail:
                            if (string.IsNullOrWhiteSpace(customMessage))
                                _onLogHandler(LogLevels.Information, Format("Failed call {0}.", message));
                            else
                                _onLogHandler(LogLevels.Information, Format("Failed call {0}, details: {1}.",
                                    message, customMessage));
                            return;
                        case ResultActionType.Default:
                            break;
                        default:
                            throw new NotSupportedException(string.Format("ResultAction {0} not supported.", r));
                    }
                }
                message = Format(AfterCallingLiteral, message);
                if (string.IsNullOrWhiteSpace(customMessage))
                    _onLogHandler(LogLevels.Information, Format(AfterCallingLiteral, message));
                else
                    _onLogHandler(LogLevels.Information, Format("Leaving {0}, details: {1}.", message, customMessage));
            }
            catch (System.Exception exc)
            {
                throw new TraceException(Format("Failed calling OnLog event, details: {0}.", message), exc);
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

        private const string BeforeCallingLiteral = "Entering {0}.";
        private const string AfterCallingLiteral = "Leaving {0}.";
    }
}
