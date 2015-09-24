using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing
{
    /// <summary>
    /// Use TracerScope to utilize the "using" c# feature to implement
    /// InvokeWrapper's (or Tracer) OnEnter and OnLeave event calls
    /// in the constructor and dispose members respectively.
    /// 
    /// This is a somewhat elegant way to utilize InvokeWrapper but
    /// minue the Exception handling portion. Your code has to handle
    /// exception (event) handling or logging.
    /// </summary>
    public class TraceScope : Attribute, IDisposable
    {
        private InvokeEngine.InvokeWrapperBase _tracer;
        private InvokeVerbosity _verbosity;
        private bool? _rethrowOnError;
        private string _funcFootprint;
        private DateTime _startTime;

        /// <summary>
        /// Constructor primarily for use in 'Attribute usage' pattern.
        /// </summary>
        /// <param name="funcFootprint"></param>
        /// <param name="verbosity"></param>
        public TraceScope(string funcFootprint = null, InvokeVerbosity verbosity = InvokeVerbosity.Default)
            : this(Tracer.Instance, funcFootprint, verbosity, null)
        { }

        /// <summary>
        /// Constructor expects parameters for use in calling method Invoke function.
        /// </summary>
        /// <param name="verbosity"></param>
        /// <param name="rethrowOnError"></param>
        /// <param name="funcFootprint"></param>
        public TraceScope(InvokeEngine.InvokeWrapperBase tracer,
            string funcFootprint = null,
            InvokeVerbosity verbosity = InvokeVerbosity.Default,
            bool? rethrowOnError = null)
        {
            if (tracer == null)
                throw new ArgumentNullException("tracer");
            _tracer = tracer;
            _verbosity = verbosity;
            funcFootprint = tracer.HandleEmptyFootprint(funcFootprint);
            _funcFootprint = funcFootprint;
            _rethrowOnError = rethrowOnError;
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
            _tracer.OnLeaveHandler(Result, _funcFootprint, _tracer.GetRunTime(_startTime));
            _tracer = null;
        }
    }
}
