using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing.InvokeEngine
{
    /// <summary>
    /// Event Type.
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// OnEnter event is raised before calling the method.
        /// </summary>
        OnEnter,
        /// <summary>
        /// OnLeave event is raised aftrer calling the method.
        /// </summary>
        OnLeave,
        /// <summary>
        /// OnException event is raised when an exception is 
        /// detected while calling the method.
        /// </summary>
        OnException
    }
}
