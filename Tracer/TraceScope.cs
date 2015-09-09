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
    public class TraceScope : IDisposable
    {
        private InvokeEngine.InvokeWrapperBase _tracer;
        private InvokeVerbosity _verbosity;
        private bool? _rethrowOnError;
        private string _funcFootprint;
        private DateTime _startTime;
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
            tracer.HandleEmptyFootprint(funcFootprint);
            _funcFootprint = funcFootprint;
            _rethrowOnError = rethrowOnError;
            _tracer.OnEnterHandler(funcFootprint);
            if (_tracer.IsSet(verbosity, InvokeVerbosity.NoRunTimeMeasurement))
                return;
            _startTime = DateTime.Now;
        }
        public void Dispose()
        {
            if (_tracer == null)
                return;
            _tracer.OnLeaveHandler(null, _funcFootprint, _tracer.GetRunTime(_startTime));
            _tracer = null;
        }
    }
}
