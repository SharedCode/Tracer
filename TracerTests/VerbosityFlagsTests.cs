using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracing.Tests
{
    [TestClass]
    public class VerbosityFlagsTests
    {
        Tracer Tracer;

        [TestInitialize]
        public void Initialize()
        {
            Tracer = new Tracer();
            Tracer.OnLog += Tracer_OnLog;
        }



        private void Tracer_OnLog(LogLevels logLevel, string message)
        {
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
