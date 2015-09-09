using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracing.Tests
{
    [TestClass]
    public class TracerNegativeTests
    {
        Tracer Tracer;
        [TestInitialize]
        public void Initialize()
        {
            Tracer = new Tracer();
        }

        [TestMethod]
        public void NegativeTest()
        {
            // note: this is the only negative condition in Tracer bcoz it uses "generics" which generates
            // compile time errors for any wrong parameter type. Passing null on reference type is the only case.
            Assert.IsTrue(Tracer.Invoke(HelloWorld, (string)null));
        }

        private static bool HelloWorld(string arg1)
        {
            return true;
        }
    }
}
