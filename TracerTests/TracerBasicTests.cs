using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracing.Tests
{
    [TestClass]
    public class TracerBasicTests
    {
        Tracer Tracer;
        Tracer TracerLogThrows;

        [TestInitialize]
        public void Initialize()
        {
            Tracer = new Tracer();
            TracerLogThrows = new Tracer();
            TracerLogThrows.OnLog += TracerLogThrows_OnLog;
        }

        private void TracerLogThrows_OnLog(LogLevels logLevel, string message)
        {
            // simulate an exception while logging. Like when logging hasn't been configured.
            throw new System.Exception("OnLog");
        }

        [TestMethod]
        public void MethodInvokeTest()
        {
            Assert.IsTrue(Tracer.Invoke(HelloWorld));
            Assert.IsTrue(Tracer.Invoke(HelloWorld, true));
            Assert.IsTrue(Tracer.Invoke(HelloWorld, true, 123));
            Assert.IsTrue(Tracer.Invoke(HelloWorld, true, 123, "foo"));
            Assert.IsTrue(Tracer.Invoke(HelloWorld, true, 123, "foo", (float)1.1));
        }

        [TestMethod]
        public void MethodInvokeVoidTest()
        {
            // basic positive tests.
            Tracer.InvokeVoid(Foo);
            Tracer.Invoke(Foo, true);
            Tracer.Invoke(Foo, true, 123);
            Tracer.Invoke(Foo, true, 123, "foo");
            Tracer.Invoke(Foo, true, 123, "foo", (float)1.1);
        }
        [TestMethod]
        [ExpectedException(typeof(TraceException), "Expected OnEnter Log to throw.")]
        public void ExceptionPreCallTest()
        {
            // test exception on pre-call log call.
            TracerLogThrows.InvokeVoid(Foo, funcFootprint: "Foo()");
        }
        [TestMethod]
        [ExpectedException(typeof(TraceException), "Expected OnLeave Log to throw.")]
        public void ExceptionPostCallTest()
        {
            // test exception post-call log call.
            TracerLogThrows.InvokeVoid(Foo, InvokeVerbosity.OnLeave, funcFootprint: "Foo()");
        }
        [TestMethod]
        [ExpectedException(typeof(System.Exception), "Expected FooThrow() to throw.")]
        public void ExceptionOnCallTest()
        {
            // test exception on-call.
            Tracer.InvokeVoid(FooThrow, InvokeVerbosity.OnException, funcFootprint: "FooThrow()");
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
