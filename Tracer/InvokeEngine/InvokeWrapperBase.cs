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
    /// <summary>
    /// On Exception Delegate.
    /// </summary>
    /// <param name="exc"></param>
    /// <param name="funcFootprint"></param>
    public delegate void OnExceptionDelegate(Exception exc, string funcFootprint);
    /// <summary>
    /// OnEvent exception delegate.
    /// </summary>
    /// <param name="exc"></param>
    /// <param name="eventType"></param>
    /// <param name="message"></param>
    public delegate void OnEventExceptionDelegate(EventType eventType, Exception exc, string message);
    /// <summary>
    /// OnEnter delegate.
    /// </summary>
    /// <param name="funcFootprint"></param>
    public delegate void OnEnterDelegate(string funcFootprint);
    /// <summary>
    /// OnLeave event delegate.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="result">Result of the method invoked or null if it is void method.</param>
    /// <param name="funcFootprint"></param>
    /// <param name="runTime"></param>
    public delegate void OnLeaveDelegate(object result, string funcFootprint, TimeSpan? runTime);
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


        #region Logging
        internal protected virtual bool OnExceptionHandler(System.Exception exc, string funcFootprint,
            InvokeVerbosity verbosity = InvokeVerbosity.Default)
        {
            IsSuccessful = false;
            Exception = exc;
            //// only call exception if verbosity flag for it is set.
            //if (!IsSet(verbosity, InvokeVerbosity.OnException))
            //    return;
            if (_onExceptionHandler != null)
            {
                _onExceptionHandler(exc, funcFootprint);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Implement this to handle event handling exceptions.
        /// </summary>
        /// <param name="exc"></param>
        virtual protected void OnEventExceptionHandler(EventType eventType, Exception exc, string funcFootprint)
        {
            if (_onEventExceptionHandler != null)
            {
                _onEventExceptionHandler(eventType, exc, funcFootprint);
                return;
            }
            throw exc;
        }
        #endregion
    }
}
