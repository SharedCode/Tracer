// Tracer v1.0
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace Tracing.InvokeEngine
{
    #region Event Delegates
    /// <summary>
    /// On Exception Delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="exc"></param>
    /// <param name="funcFootprint"></param>
    public delegate void OnExceptionDelegate(InvokeWrapperBase sender, Exception exc, string funcFootprint);
    /// <summary>
    /// OnEvent exception delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventType"></param>
    /// <param name="exc"></param>
    /// <param name="message"></param>
    public delegate void OnEventExceptionDelegate(InvokeWrapperBase sender, EventType eventType, Exception exc, string message);
    /// <summary>
    /// OnEnter delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="funcFootprint"></param>
    public delegate void OnEnterDelegate(InvokeWrapperBase sender, string funcFootprint);
    /// <summary>
    /// OnLeave event delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    /// <param name="funcFootprint"></param>
    /// <param name="runTime"></param>
    public delegate void OnLeaveDelegate(InvokeWrapperBase sender, object result, string funcFootprint, TimeSpan? runTime);
    #endregion

    /// <summary>
    /// Invoke Wrapper Base.
    /// </summary>
    public partial class InvokeWrapperBase
    {
        /// <summary>
        /// OnException event is raised when an unhandled exception was detected while 
        /// running the submitted function.
        /// </summary>
        public event OnExceptionDelegate OnException
        {
            add { _onExceptionHandler += value; }
            remove { _onExceptionHandler -= value; }
        }
        private event OnExceptionDelegate _onExceptionHandler;

        /// <summary>
        /// OnEventException event is raised when an unhandled exception was detected 
        /// while raising any of the supported Events (OnEnter, OnLeave).
        /// </summary>
        public event OnEventExceptionDelegate OnEventException
        {
            add { _onEventExceptionHandler += value; }
            remove { _onEventExceptionHandler -= value; }
        }
        private event OnEventExceptionDelegate _onEventExceptionHandler;

        /// <summary>
        /// Checks whether a certain invoke verbosity flag is set.
        /// </summary>
        /// <param name="verbosity">Invoke Verbosity set by user or got by default.</param>
        /// <param name="flagToCheck">Invoke Verbosity flag to check.</param>
        /// <returns></returns>
        internal protected bool IsSet(InvokeVerbosity verbosity, InvokeVerbosity flagToCheck)
        {
            return (InvokeVerbosity)(verbosity & flagToCheck) == flagToCheck;
        }

        /// <summary>
        /// true will raise Tracing events such as OnEnter, OnLeave, OnException.
        /// </summary>
        virtual public bool InvokeTracing
        {
            get { return _invokeTracing; }
            set { _invokeTracing = value; }
        }
        private bool _invokeTracing = true;


        #region Exception events
        internal protected virtual bool OnExceptionHandler(System.Exception exc, string funcFootprint,
            InvokeVerbosity verbosity = InvokeVerbosity.Default)
        {
            IsSuccessful = false;
            Exception = exc;
            //// only call exception if verbosity flag for it is set.
            //if (!IsSet(verbosity, InvokeVerbosity.OnException))
            //    return;
            if (_onExceptionHandler == null)
                return false;
            _onExceptionHandler(this, exc, funcFootprint);
            return true;
        }
        /// <summary>
        /// Implement this to handle on event exception handling.
        /// </summary>
        /// <param name="exc"></param>
        virtual protected void OnEventExceptionHandler(EventType eventType, Exception exc, string funcFootprint)
        {
            if (_onEventExceptionHandler == null)
                throw exc;
            _onEventExceptionHandler(this, eventType, exc, funcFootprint);
        }
        #endregion
    }
}
