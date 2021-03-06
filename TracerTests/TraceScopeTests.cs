﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracing.Tests
{
    [TestClass]
    public class TraceScopeTests
    {
        Tracer Tracer;
        int logCallCount;
        [TestInitialize]
        public void Initialize()
        {
            Tracer = new Tracer();
            Tracer.OnLog += Tracer_OnLog;
        }

        private void Tracer_OnLog(LogLevels logLevel, string[] category, string message)
        {
            logCallCount++;
        }

        [TestMethod]
        public void BasicTest()
        {
            // using TraceScope will implicitly call OnEnter and OnLeave event loggers
            // in ctor and Dispose methods of TraceScope.
            using (new TraceScope(Tracer, "HelloWorld(\"hi\")"))
            {
                HelloWorld("hi");
            }
            // logCallCount should be 2 as Tracer_OnLog should had been invoked two times.
            // One OnEnter and a 2nd OnLeave event.
            Assert.IsTrue(logCallCount == 2);

            // invoke lambda expression code snippet
            logCallCount = 0;
            Tracer.InvokeVoid(
                () =>
                {
                    HelloWorld("hi");
                },
                funcFootprint: "HelloWorld(\"hi\")"
                );
            Assert.IsTrue(logCallCount == 2);
            logCallCount = 0;

            // call method via Tracer.Invoke.
            Tracer.Invoke(HelloWorld, "hi", funcFootprint: "HelloWorld(\"hi\")");
            Assert.IsTrue(logCallCount == 2);
        }

        private static bool HelloWorld(string arg1)
        {
            return true;
        }
    }
}
