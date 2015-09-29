// Tracer v1.0
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace Tracing.InvokeEngine
{
    /// <summary>
    /// Invoke Wrapper for general purpose use.
    /// </summary>
    public partial class InvokeWrapper : InvokeWrapperBase
    {
        /// <summary>
        /// OnEnter event is raised before calling the submitted function for invoke.
        /// </summary>
        public event OnEnterDelegate OnEnter
        {
            add { _onEnterHandler += value; }
            remove { _onEnterHandler -= value; }
        }
        private event OnEnterDelegate _onEnterHandler;
        /// OnLeave event is raised after calling the submitted function for invoke.
        public event OnLeaveDelegate OnLeave
        {
            add { _onLeaveHandler += value; }
            remove { _onLeaveHandler -= value; }
        }
        private event OnLeaveDelegate _onLeaveHandler;
        internal protected override void OnEnterHandler(string functionInfo)
        {
            if (_onEnterHandler == null) return;
            try
            {
                _onEnterHandler(functionInfo);
            }
            catch (Exception exc)
            {
                throw new TraceException(MessageStrings.OnEnterEventFailedCall, exc);
            }
        }
        internal protected override void OnLeaveHandler(object result, string functionInfo, TimeSpan? runTime)
        {
            if (_onLeaveHandler == null) return;
            try
            {
                _onLeaveHandler(result, functionInfo, runTime);
            }
            catch (Exception exc)
            {
                throw new TraceException(MessageStrings.OnLeaveEventFailedCall, exc);
            }
        }
    }
}
