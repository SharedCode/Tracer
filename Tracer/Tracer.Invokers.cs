using System;
using Tracing.InvokeEngine;

namespace Tracing
{
    /// <summary>
    /// Tracer is work in progress, it is supposed to simplify
    /// function call Log tracing and error handling.
    /// </summary>
    public partial class Tracer : InvokeWrapperBase
    {
        #region Function delegates with different "Generic" parameters and return.

        public delegate bool Func(out string message);
        public delegate bool Func<in T>(T arg, out string message);
        public delegate bool Func<in T1, in T2>(T1 arg1, T2 arg2, out string message);
        public delegate bool Func<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3, out string message);
        public delegate bool Func<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out string message);
        public delegate bool Func<in T1, in T2, in T3, in T4, in T5>(
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out string message);
        #endregion

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Tracer() : this(null) { }
        public Tracer(string[] category = null,
            ResultEvaluatorDelegate resultEvaluator = null)
        {
            ResultEvaluator = resultEvaluator;
            // set the log Category.
            Category = new string[] { DefaultCategory };
            if (category != null)
                Category = category;
        }

        #region Invoke Log Fail
        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeLogFail(Func func, out string message, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method) : HandleEmptyFootprint(funcFootprint);
            return invokeLogFail(func, out message, s, verbosity);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeLogFail<T>(Func<T> func, T arg, out string message, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var funcParamText = funcFootprint == null ? Format(func.Method, verbosity, arg) : HandleEmptyFootprint(funcFootprint);
            Func f = (out string s) => { return func(arg, out s); };
            return invokeLogFail(f, out message, funcParamText, verbosity, rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeLogFail<T1, T2>(Func<T1, T2> func, T1 arg1, T2 arg2, out string message,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var funcParamText = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2) : HandleEmptyFootprint(funcFootprint);
            Func f = (out string s) => { return func(arg1, arg2, out s); };
            return invokeLogFail(f, out message, funcParamText, verbosity, rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeLogFail<T1, T2, T3>(Func<T1, T2, T3> func, T1 arg1, T2 arg2, T3 arg3,
            out string message, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var funcParamText = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3) : HandleEmptyFootprint(funcFootprint);
            Func f = (out string s) => { return func(arg1, arg2, arg3, out s); };
            return invokeLogFail(f, out message, funcParamText, verbosity, rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeLogFail<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
                                                  out string message, InvokeVerbosity verbosity = InvokeVerbosity.Default,
                                                  bool? rethrowOnError = null, string funcFootprint = null)
        {
            var funcParamText = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4) :
                                                        HandleEmptyFootprint(funcFootprint);
            Func f = (out string s) => { return func(arg1, arg2, arg3, arg4, out s); };
            return invokeLogFail(f, out message, funcParamText, verbosity, rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeLogFail<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
                                                      T5 arg5, out string message, InvokeVerbosity verbosity = InvokeVerbosity.Default,
                                                      bool? rethrowOnError = null, string funcFootprint = null)
        {
            var funcParamText = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4, arg5) :
                                            HandleEmptyFootprint(funcFootprint);
            Func f = (out string s) => { return func(arg1, arg2, arg3, arg4, arg5, out s); };
            return invokeLogFail(f, out message, funcParamText, verbosity, rethrowOnError);
        }

        #endregion

        #region "internal" helper invoke
        private bool invokeLogFail(Func func, out string message, string funcFootprint,
            InvokeVerbosity verbosity = InvokeVerbosity.Default, bool? rethrowOnError = null)
        {
            string s = null;
            var r = invoke(() => { return func(out s); }, funcFootprint, true,
                (InvokeVerbosity)verbosity, rethrowOnError);
            message = s;
            if (!r)
                LogError(funcFootprint, message);
            return r;
        }
        #endregion

        internal protected override void OnEnterHandler(string funcFootprint)
        {
            if (_onLogHandler == null) return;
            try
            {
                _onLogHandler(_tracingLogLevel, Category, Format(MessageStrings.EnteringMessageTemplate, funcFootprint));
            }
            catch (Exception exc)
            {
                throw new TraceException(MessageStrings.OnEnterEventFailedCall, exc);
            }
        }
        internal protected override void OnLeaveHandler(object result, string funcFootprint, TimeSpan? runTime)
        {
            if (_onLeaveHandler != null)
            {
                if (ResultEvaluator != null)
                    throw new NotSupportedException(MessageStrings.UnsupportedResultEvaluatorAndOnLeave);
                try
                {
                    _onLeaveHandler(result, funcFootprint, runTime);
                    return;
                }
                catch (Exception exc)
                {
                    throw new TraceException(MessageStrings.OnLeaveEventFailedCall, exc);
                }
            }
            if (_onLogHandler == null) return;
            var message = funcFootprint;
            try
            {
                if (runTime != null)
                    message = string.Format(MessageStrings.MessageWithRunTimeMessageTemplate, funcFootprint, runTime.Value.TotalSeconds);
                string customMessage = "";
                if (ResultEvaluator != null)
                {
                    ResultActionType r = ResultActionType.Default;
                    try
                    {
                        r = ResultEvaluator(result, runTime, out customMessage);
                    }
                    catch (System.Exception exc)
                    {
                        throw new TraceException(Format(MessageStrings.ResultEvaluatorCallFailedMessageTemplate, message), exc);
                    }
                    switch (r)
                    {
                        case ResultActionType.None:
                            return;
                        case ResultActionType.Pass:
                            if (string.IsNullOrWhiteSpace(customMessage))
                                _onLogHandler(LogLevels.Information, Category, Format(MessageStrings.SuccessfulCallMessageTemplate, message));
                            else
                                _onLogHandler(LogLevels.Information, Category, Format(MessageStrings.SuccessfulCallWithDetailsMessageTemplate,
                                    message, customMessage));
                            return;
                        case ResultActionType.Fail:
                            if (string.IsNullOrWhiteSpace(customMessage))
                                _onLogHandler(LogLevels.Information, Category, Format(MessageStrings.FailedCallMessageTemplate, message));
                            else
                                _onLogHandler(LogLevels.Information, Category, Format(MessageStrings.FailedCallWithDetailsMessageTemplate,
                                    message, customMessage));
                            return;
                        case ResultActionType.Default:
                            break;
                        default:
                            throw new NotSupportedException(string.Format(MessageStrings.UnsupportedResultActionMessageTemplate, r));
                    }
                }
                if (string.IsNullOrWhiteSpace(customMessage))
                    _onLogHandler(_tracingLogLevel, Category, Format(MessageStrings.LeavingMessageTemplate, message));
                else
                    _onLogHandler(_tracingLogLevel, Category, Format(MessageStrings.LeavingWithDetailsMessageTemplate, message, customMessage));
            }
            catch (System.Exception exc)
            {
                throw new TraceException(Format(MessageStrings.FailedCallMessageTemplate, message), exc);
            }
        }
        internal override protected string HandleEmptyFootprint(string footprint)
        {
            if (footprint == string.Empty)
                return MessageStrings.NoFunctionInfo;
            return footprint;
        }

        private void LogError(string funcFootprint, string message)
        {
            IsSuccessful = false;
            if (_onLogHandler == null) return;
            var errMessage = Format(MessageStrings.FailedCallWithDetailsMessageTemplate, funcFootprint, message);
            try
            {
                _onLogHandler(LogLevels.Error, Category, errMessage);
            }
            catch (Exception exc)
            {
                OnEventExceptionHandler(EventType.OnLeave, exc, funcFootprint);
            }
        }
    }
}
