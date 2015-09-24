using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracing.InvokeEngine;

namespace Tracing
{
    public class TraceScope : TraceScope<InvokeWrapperBase>
    {
        /// <summary>
        /// Constructor that uses singleton Tracer.Instance.
        /// NOTE: will throw if Tracer.Instance is null.
        /// </summary>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        public TraceScope(string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            string[] category = null)
            : this(Tracing.Tracer.Instance, funcFootprint, verbosity, null, category)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tracer">A valid Tracer instance.</param>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        public TraceScope(InvokeWrapperBase tracer, string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null,
            string[] category = null)
            : base(tracer, funcFootprint, verbosity, rethrowOnError, category)
        {
            if (tracer == null)
                throw new ArgumentNullException("tracer");
        }
    }

    /// <summary>
    /// Use TraceScope to utilize the "using" c# feature to implement
    /// InvokeWrapper's (or Tracer) OnEnter and OnLeave event calls
    /// in the constructor and dispose members respectively.
    /// 
    /// This is a somewhat elegant way to utilize InvokeWrapper but
    /// minus the Exception handling portion. Your code has to handle
    /// exception (event) handling or logging.
    /// </summary>
    public class TraceScope<T> : IDisposable
    where T : InvokeWrapperBase, new()
    {
        private InvokeWrapperBase _tracer;
        private InvokeVerbosity _verbosity;
        private bool? _rethrowOnError;
        private string _funcFootprint;
        private DateTime _startTime;
        private string[] _tracerCategory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        public TraceScope(string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            string[] category = null)
            : this(null, funcFootprint, verbosity, null, category)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tracer"></param>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        public TraceScope(T tracer,
            string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null,
            string[] category = null)
        {
            if (tracer == null)
                tracer = new T();
            _tracer = tracer;
            _verbosity = verbosity;
            funcFootprint = tracer.HandleEmptyFootprint(funcFootprint);
            _funcFootprint = funcFootprint;
            _rethrowOnError = rethrowOnError;
            if (_tracer is Tracer)
            {
                if (category == null)
                    category = new string[] { Tracing.Tracer.DefaultCategory };
                _tracerCategory = ((Tracer)_tracer).Category;
                ((Tracer)_tracer).Category = category;
            }
            if (_tracer.IsSet(verbosity, InvokeVerbosity.OnEnter))
                _tracer.OnEnterHandler(funcFootprint);
            if (_tracer.IsSet(verbosity, InvokeVerbosity.NoRunTimeMeasurement))
                return;
            _startTime = DateTime.Now;
        }
        /// <summary>
        /// Set to the result of the block or return of the method call.
        /// NOTE: this value will be submitted as parameter to the OnLeave event handler
        /// to allow it to evaluate what to do based on the Result.
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            if (_tracer == null)
                return;
            if (_tracer.IsSet(_verbosity, InvokeVerbosity.OnLeave))
                _tracer.OnLeaveHandler(Result, _funcFootprint, _tracer.GetRunTime(_startTime));
            if (_tracer is Tracer)
            {
                ((Tracer)_tracer).Category = _tracerCategory;
            }
            _tracer = null;
        }
    }
}
