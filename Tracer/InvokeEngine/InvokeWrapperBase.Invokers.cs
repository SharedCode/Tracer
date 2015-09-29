using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tracing.InvokeEngine
{
    /// <summary>
    /// Invoke Wrapper Base contains general purpose method
    /// runner. It manages raising events before, after and
    /// when an exception occurs when running a method or function.
    /// </summary>
    public partial class InvokeWrapperBase
    {
        #region Function delegates with different "Generic" parameters and return.
        // void Funcs
        public delegate void VoidFunc();
        public delegate void VoidFunc<in T1>(T1 arg1);
        public delegate void VoidFunc<in T1, in T2>(T1 arg1, T2 arg2);
        public delegate void VoidFunc<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate void VoidFunc<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        public delegate void VoidFunc<in T1, in T2, in T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        public delegate void VoidFunc<in T1, in T2, in T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg);
        #endregion

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public InvokeWrapperBase()
        {
            IsSuccessful = true;
        }

        #region Invoke

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError">true will rethrow unhandled exception detected after it gets logged,
        /// otherwise it will just be logged (and thus, suppressed).</param>
        /// <param name="funcFootprint">Function call footprint. Pass your custom formatted
        /// method call 'footprint' (method name and its parameter values nicely formatted) text or null.
        /// If null, the virtual Format function will be invoked to allow derived classes to 
        /// implement generation of function call footprint.</param>
        /// <returns></returns>
        public TResult Invoke<TResult>(System.Func<TResult> func, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method) : HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public TResult Invoke<T, TResult>(System.Func<T, TResult> func, T arg, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg) : HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(arg); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public TResult Invoke<T1, T2, TResult>(System.Func<T1, T2, TResult> func, T1 arg1, T2 arg2,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2) : HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(arg1, arg2); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public TResult Invoke<T1, T2, T3, TResult>(System.Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3) : HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(arg1, arg2, arg3); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }

        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <returns></returns>
        public TResult Invoke<T1, T2, T3, T4, TResult>(System.Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2,
                            T3 arg3, T4 arg4, InvokeVerbosity verbosity = InvokeVerbosity.Default,
                            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4) :
                                            HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(arg1, arg2, arg3, arg4); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }
        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <returns></returns>
        public TResult Invoke<T1, T2, T3, T4, T5, TResult>(System.Func<T1, T2, T3, T4, T5, TResult> func, T1 arg1, T2 arg2,
                            T3 arg3, T4 arg4, T5 arg5, InvokeVerbosity verbosity = InvokeVerbosity.Default,
                            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4, arg5) :
                                            HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(arg1, arg2, arg3, arg4, arg5); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }
        /// <summary>
        /// Invoke function, (optional)do tracing, process/log any exception generated.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <returns></returns>
        public TResult Invoke<T1, T2, T3, T4, T5, T6, TResult>(System.Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 arg1, T2 arg2,
                            T3 arg3, T4 arg4, T5 arg5, T6 arg6, InvokeVerbosity verbosity = InvokeVerbosity.Default,
                            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4, arg5, arg6) :
                                            HandleEmptyFootprint(funcFootprint);
            return invoke(() => { return func(arg1, arg2, arg3, arg4, arg5, arg6); }, s, verbosity: verbosity, rethrowOnError: rethrowOnError);
        }

        #endregion

        #region InvokeVoid
        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void InvokeVoid<T1, T2>(VoidFunc<T1, T2> func, T1 arg1, T2 arg2,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(arg1, arg2); }, s, verbosity, rethrowOnError);
        }
        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void InvokeVoid<T1, T2, T3>(VoidFunc<T1, T2, T3> func, T1 arg1, T2 arg2, T3 arg3,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(arg1, arg2, arg3); }, s, verbosity, rethrowOnError);
        }
        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        /// <param name="funcFootprint"></param>
        public void InvokeVoid<T1, T2, T3, T4>(VoidFunc<T1, T2, T3, T4> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(arg1, arg2, arg3, arg4); }, s, verbosity, rethrowOnError);
        }
        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        /// <param name="funcFootprint"></param>
        public void InvokeVoid<T1, T2, T3, T4, T5>(VoidFunc<T1, T2, T3, T4, T5> func, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4, arg5) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(arg1, arg2, arg3, arg4, arg5); }, s, verbosity, rethrowOnError);
        }

        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="arg6"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        /// <param name="funcFootprint"></param>
        public void InvokeVoid<T1, T2, T3, T4, T5, T6>(VoidFunc<T1, T2, T3, T4, T5, T6> func, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, T6 arg6,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1, arg2, arg3, arg4, arg5, arg6) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(arg1, arg2, arg3, arg4, arg5, arg6); }, s, verbosity, rethrowOnError);
        }

        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <param name="func"></param>
        public void InvokeVoid(VoidFunc func, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(); }, s, verbosity, rethrowOnError);
        }
        /// <summary>
        /// Invoke a method that returns nothing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        public void InvokeVoid<T1>(VoidFunc<T1> func, T1 arg1, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null, string funcFootprint = null)
        {
            var s = funcFootprint == null ? Format(func.Method, verbosity, arg1) :
                                            HandleEmptyFootprint(funcFootprint);
            invokeVoid(() => { func(arg1); }, s, verbosity, rethrowOnError);
        }
        #region Formatters

        /// <summary>
        /// Utility function for Formatting text msg and optional messageArgs.
        /// </summary>
        /// <param name="messageFormat">Text message containing formatting section.</param>
        /// <param name="messageArgs">Arguments to be rendered/formatted part of the Text message.</param>
        /// <returns>Actual formatted text that got logged.</returns>
        public static string Format(string messageFormat, params object[] messageArgs)
        {
            string data;
            if (messageFormat != null && messageArgs != null && messageArgs.Length > 0)
                data = string.Format(messageFormat, messageArgs);
            else
                data = messageFormat;
            return data;
        }

        protected virtual string Format(MethodInfo method, string formattedMethodParams = null)
        {
            // Retrieving method name using Declaring type is found to be quick, no perf issue.
            // It is, however, recommended to pass in function call footprint text which is the more optimal approach.
            var className = method.DeclaringType.ReflectedType != null ? method.DeclaringType.ReflectedType.Name : method.DeclaringType.Name;
            var funcName = string.Format("{0}.{1}", className, method.Name);
            if (string.IsNullOrWhiteSpace(formattedMethodParams))
                return funcName + "()";
            return string.Format("{0}({1})", funcName, formattedMethodParams);
        }
        protected virtual string Format(MethodInfo method,
            InvokeVerbosity verbosity, params object[] values)
        {
            var funcName = Format(method);
            var functionWithParams = funcName;
            if (values != null && values.Length > 0)
            {
                if (IsSet(verbosity, InvokeVerbosity.HideParameterValues))
                    return string.Format("{0}(<omitted>)", funcName);
                // Format function & parameter(s) into a nice, readable text.
                // NOTE: merely converts each parameter to CSV string for quick formatting, no Reflection.
                functionWithParams = string.Format("{0}({1})", funcName, string.Join(",", values.Select(ArgToString).ToArray()));
                return functionWithParams;
            }
            return functionWithParams + "()";
        }
        private static string ArgToString(object arg)
        {
            if (arg == null) return "null";
            if (arg is string) return string.Format("\"{0}\"", (string)arg);
            if (arg is char) return string.Format("'{0}'", arg);
            if (arg is IEnumerable)
            {
                var sb = new StringBuilder();
                var c = 0;
                foreach (var o in (IEnumerable)arg)
                {
                    c++;
                    if (sb.Length == 0)
                        sb.Append(o.ToString());
                    else
                        sb.AppendFormat(", {0}", o.ToString());
                }
                return c > 1 ? string.Format("'{0}'", sb.ToString()) : sb.ToString();
            }
            return arg.ToString();
        }
        #endregion
        #endregion

        /// <summary>
        /// true will cause rethrow when exception is encountered.
        /// </summary>
        public bool Rethrow
        {
            get
            {
                return (_rethrow == null && _onExceptionHandler == null) || _rethrow.Value ? true : false;
            }
            set
            {
                _rethrow = value;
            }
        }
        private bool? _rethrow;

        /// <summary>
        /// Message Strings contain all the messages emitted by InvokeWrapperBase derived classes.
        /// Update this to your message strings structure if you have defined your custom messages
        /// and would like to override the default messages emitted by Tracer.
        /// </summary>
        static public MessageStrings MessageStrings = new MessageStrings();

        /// <summary>
        /// Contains the exception encountered after the last Invoke
        /// was done or null if there isn't any generated.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// true if last invoke is seemingly successful.
        /// Criteria to be set to false(failed), anyone of below will cause IsSuccessful to return false:
        /// - If Invoke threw exception
        /// - If Invoked function via InvokeLogFail call returned false
        /// </summary>
        public bool IsSuccessful
        {
            get { return _isSuccessful; }
            protected set
            {
                _isSuccessful = value;
                if (value)
                {
                    Exception = null;
                }
            }
        }
        /// <summary>
        /// Set singleton Instance to an instance.
        /// Thread safe.
        /// </summary>
        /// <param name="tracer"></param>
        /// <param name="throwIfSet"></param>
        public static void SetTracer(InvokeWrapperBase tracer, bool throwIfSet = false)
        {
            if (tracer == null)
                throw new ArgumentNullException("tracer");
            var msg = "InvokeWrapperBase.Instance is already set.";
            if (Instance != null && throwIfSet)
                throw new InvalidOperationException(msg);
            lock(_locker)
            {
                if (Instance != null && throwIfSet)
                    throw new InvalidOperationException(msg);
                Instance = tracer;
            }
        }
        private static object _locker = new object();

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static InvokeWrapperBase Instance { get; private set; }

        /// <summary>
        /// false disables tracing globally.
        /// </summary>
        public static bool Enabled = true;

        #region Event Raisers
        internal protected virtual void OnEnterHandler(string functionInfo)
        {
            // do nothing.
        }
        internal protected virtual void OnLeaveHandler(object result, string functionInfo, TimeSpan? runTime)
        {
            // do nothing.
        }
        #endregion

        #region "internal" helper invoke
        protected TResult invoke<TResult>(System.Func<TResult> func, string funcFootprint,
            bool logFail = false, InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null)
        {
            if (!Enabled)
            {
                return func();
            }
            DateTime? startTime = null;
            if (!IsSet(verbosity, InvokeVerbosity.NoRunTimeMeasurement))
                startTime = DateTime.Now;
            try
            {
                IsSuccessful = true;
                if (!InvokeTracing || verbosity == InvokeVerbosity.None)
                    return func();
                // wrap the call in pre/post logs if respective flag is set.
                if (IsSet(verbosity, InvokeVerbosity.OnEnter))
                {
                    try
                    {
                        OnEnterHandler(funcFootprint);
                    }
                    catch (TraceException internalExc)
                    {
                        // call the func even if log failure occurred.
                        var r2 = func();
                        OnEventExceptionHandler(EventType.OnEnter, internalExc, funcFootprint);
                        return r2;
                    }
                }
                var r = func();
                if (logFail && !(bool)(object)r)
                    return r;
                // skip logging post call if LogOnLeave is not set.
                if (!IsSet(verbosity, InvokeVerbosity.OnLeave))
                    return r;
                try
                {
                    OnLeaveHandler(r, funcFootprint, GetRunTime(startTime));
                }
                catch (TraceException internalExc)
                {
                    OnEventExceptionHandler(EventType.OnLeave, internalExc, funcFootprint);
                }
                return r;
            }
            catch (TraceException)
            {
                throw;
            }
            catch (System.Exception exc)
            {
                OnExceptionHandler(exc, funcFootprint, verbosity);
                // a.) apply rethrow rules set in rethrowOnError parameter if provided.
                if (rethrowOnError != null)
                {
                    if (rethrowOnError.Value)
                        throw;
                    return default(TResult);
                }
                // b.) otherwise, apply this object's Rethrow rule.
                if (_onExceptionHandler == null && Rethrow) throw;
                return default(TResult);
            }
        }
        private void invokeVoid(VoidFunc func, string funcFootprint,
            InvokeVerbosity verbosity = InvokeVerbosity.Default, bool? rethrowOnError = null)
        {
            if (!Enabled)
            {
                func();
                return;
            }
            DateTime? startTime = null;
            if (!IsSet(verbosity, InvokeVerbosity.NoRunTimeMeasurement))
                startTime = DateTime.Now;
            try
            {
                IsSuccessful = true;
                if (!InvokeTracing || verbosity == InvokeVerbosity.None)
                {
                    func();
                    return;
                }
                // wrap the call in pre/post logs if respective flag is set.
                if (IsSet(verbosity, InvokeVerbosity.OnEnter))
                {
                    try
                    {
                        OnEnterHandler(funcFootprint);
                    }
                    catch (TraceException internalExc)
                    {
                        func();
                        OnEventExceptionHandler(EventType.OnEnter, internalExc, funcFootprint);
                        return;
                    }
                }
                func();
                // skip logging post call if LogOnLeave is not set.
                if (!IsSet(verbosity, InvokeVerbosity.OnLeave))
                    return;
                // handle log exception.
                try
                {
                    OnLeaveHandler(null, funcFootprint, GetRunTime(startTime));
                }
                catch (TraceException internalExc)
                {
                    OnEventExceptionHandler(EventType.OnLeave, internalExc, funcFootprint);
                    return;
                }
            }
            catch (TraceException)
            {
                throw;
            }
            catch (System.Exception exc)
            {
                OnExceptionHandler(exc, funcFootprint, verbosity);
                // a.) apply rethrow rules set in rethrowOnError parameter if provided.
                if (rethrowOnError != null)
                {
                    if (rethrowOnError.Value)
                        throw;
                    return;
                }
                // b.) otherwise, apply this object's Rethrow rule.
                if (_onExceptionHandler == null && Rethrow) throw;
            }
        }
        #endregion
        /// <summary>
        /// HandleEmptyFootprint returns a standard label if funtion footprint
        /// provided is empty string ("").
        /// </summary>
        /// <param name="footprint"></param>
        /// <returns></returns>
        internal virtual protected string HandleEmptyFootprint(string footprint)
        {
            return footprint;
        }

        internal TimeSpan? GetRunTime(DateTime? startTime)
        {
            if (startTime == null)
                return null;
            return DateTime.Now.Subtract(startTime.Value);
        }

        private bool _isSuccessful;
    }
}
