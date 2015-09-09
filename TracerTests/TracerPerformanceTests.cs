using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tracing.Tests
{
    [TestClass]
    public class TracerPerformanceTests
    {
        const int IterationCount = 5000;
        const int ThresholdDeltaTimeInMillis = 1000;
        Tracer Tracer;

        [TestInitialize]
        public void Initialize()
        {
            Tracer = new Tracer();
        }
        #region Tracer event handlers
        private string Tracer_OnLogException(LogLevels logLevel, Exception exc, string messageFormat, object[] messageArgs)
        {
            return "";
        }

        private string Tracer_OnLog(LogLevels logLevel, string messageFormat, object[] messageArgs)
        {
            return "";
        }

        private string Tracer_OnInternalException(Exception exc, string functionInfo)
        {
            return "";
        }
        #endregion

        [TestMethod]
        public void CompareCallWithNoTracerTest()
        {
            DateTime t1 = DateTime.Now;
            for(int i = 0; i < IterationCount; i++)
            {
                Foo();
            }
            DateTime t2 = DateTime.Now;
            for (int i = 0; i < IterationCount; i++)
            {
                // note: running Foo 5,000 times via Tracer only has delay of around 9 millis 
                // (depends on processor speed) as compared to running direct!
                Tracer.InvokeVoid(Foo, funcFootprint: "Foo()");
            }
            DateTime t3 = DateTime.Now;
            var deltaTime = t3.Subtract(t2).TotalMilliseconds - t2.Subtract(t1).TotalMilliseconds;
            Assert.IsTrue(deltaTime < ThresholdDeltaTimeInMillis);
        }

        #region Methods to invoke
        private static bool HelloWorld(bool arg1, int arg2, string arg3, float arg4)
        {
            return true;
        }
        private static bool HelloWorld(bool arg1, int arg2, string arg3)
        {
            return true;
        }
        private static bool HelloWorld(bool arg1, int arg2)
        {
            return true;
        }
        private static bool HelloWorld(bool arg1)
        {
            return true;
        }
        private static bool HelloWorld()
        {
            return true;
        }

        private static bool Foo(bool arg1, int arg2, string arg3, float arg4)
        {
            return true;
        }
        private static bool Foo(bool arg1, int arg2, string arg3)
        {
            return true;
        }
        private static bool Foo(bool arg1, int arg2)
        {
            return true;
        }
        private static bool Foo(bool arg1)
        {
            return true;
        }
        private static void Foo()
        {
        }
        private static void FooThrow()
        {
            throw new System.Exception("FooThrow");
        }
        #endregion
    }
}
