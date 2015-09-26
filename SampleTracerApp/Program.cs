using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using Tracing;
using Tracing.MSEnterpriseLogging;

namespace SampleTracerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize MSEL static log writer as is used by the 
            // "MSEL hooks" in Tracing MSEnterpriseLogging library.
            Logger.SetLogWriter(new LogWriterFactory().Create(), false);

            Console.WriteLine("Running Enterprise Logging Tracer demo.");
            DemoEnterpriseTracer();
            Console.WriteLine();
            Console.WriteLine("Running Enterprise Logging Trace Scope Using ResultsEval demo.");
            DemoEnterpriseTracerWithScopeAndResultEval();
            Console.WriteLine("Pls. check your MSEL log file for messages logged by Enterprise Logging demo.");
            Console.WriteLine();

            Console.WriteLine("Running Tracer with Scope and ResultsEval to Console demo.");
            DemoTracerWithScopeAndResultEval();
            Console.WriteLine();

            Console.WriteLine("Running Tracer Logging to Console demo.");
            DemoTracerToConsole();
            Console.WriteLine();

            Console.WriteLine("Running TraceScope to Console demo.");
            DemoTraceScopeToConsole();
            Console.WriteLine();

            Console.WriteLine("Running TraceScope using singleton Tracer, log to Console demo.");
            DemoTraceScopeUsingSingletonTracer();
            Console.WriteLine();

            Console.WriteLine("Press <Enter> key to Quit.");
            Console.ReadLine();
        }

        #region demo code
        private static void DemoEnterpriseTracer()
        {
            ResultEvaluatorDelegate re = (object result, TimeSpan? runTime, out string customMessage) =>
            {
                customMessage = null;
                return ResultActionType.Default;
            };

            EnterpriseTracer Tracer = new EnterpriseTracer(new string[] { "DemoEnterpriseTracer" }, re);
            Tracer.Invoke(Foo, funcFootprint: "Foo()");
        }

        private static void DemoEnterpriseTracerWithScopeAndResultEval()
        {
            // Shows how to dictate generated log message using a ResultsEvaluator.
            EnterpriseTracer Tracer = new EnterpriseTracer(new string[] { "DemoEnterpriseTracerWithScopeAndResultEval" }, resultEval);
            using (new Tracing.MSEnterpriseLogging.TraceScope(Tracer, "FooTakesTime(5)"))
            {
                FooTakesTime(5);
            }
        }
        private static void DemoTracerWithScopeAndResultEval()
        {
            // Shows how to dictate generated log message using a ResultsEvaluator.
            var Tracer = new Tracing.Tracer(new string[] { "DemoTracerWithScopeAndResultEval" }, resultEval);
            Tracer.OnLog += Tracer_OnLog;
            Tracer.OnException += Tracer_OnException;
            using (new Tracing.TraceScope(Tracer, "FooTakesTime(4)"))
            {
                FooTakesTime(4);
            }
        }

        private static void DemoTraceScopeToConsole()
        {
            // hook up console writer/logger for this demo!
            var Tracer = new Tracing.Tracer(category: new string[] { "DemoTraceScopeToConsole" });
            Tracer.OnLog += Tracer_OnLog;
            Tracer.OnException += Tracer_OnException;
            using (new Tracing.TraceScope(Tracer, "Foo()"))
            {
                // invoke Foo via Tracer to generate code tracing useful logs on Enter, [on Exit].
                // run time measurement is also logged on generated "on Exit" logs.
                Foo();
            }
            // invoke FooThatThrows.
            var i = 123;
            try
            {
                using (new Tracing.TraceScope(Tracer, string.Format("FooThatThrows({0})", i),
                    InvokeVerbosity.OnEnter))
                {
                    FooThatThrows(i);
                    // NOTE: OnLeave will not be invoked (i.e. - no OnLeave log generated)
                    // because verbosity is set to "OnEnter" only.
                }
            }
            catch
            {
                Console.WriteLine("Caught an exception.");
            }
        }


        private static void DemoTraceScopeUsingSingletonTracer()
        {
            // hook up console writer/logger for this demo!
            var Tracer = new Tracing.Tracer(category: new string[] { "DemoTraceScopeUsingSingletonTracer" });
            Tracer.OnLog += Tracer_OnLog;
            Tracer.OnException += Tracer_OnException;
            // Call SetTracer to setup singleton Tracer Instance.
            // Setting up the singleton Tracer needs to be done once in App bootup.
            Tracing.Tracer.SetTracer(Tracer);

            // Using Tracing.TraceScope passing no Tracer instance param 
            // will use the singleton Tracer instance.
            using (new Tracing.TraceScope("Foo()"))
            {
                // invoke Foo via Tracer to generate code tracing useful logs on Enter.
                // run time measurement is also logged on generated "on Exit" logs.
                Foo();
            }
            // invoke FooThatThrows.
            var i = 123;
            try
            {
                using (new Tracing.TraceScope(string.Format("FooThatThrows({0})", i)))
                {
                    FooThatThrows(i);
                }
            }
            catch
            {
                Console.WriteLine("Caught an exception.");
            }
        }

        private static void DemoTracerToConsole()
        {
            // hook up console writer/logger for this demo!
            var Tracer = new Tracing.Tracer(category: new string[] { "DemoTracerToConsole" });
            Tracer.OnLog += Tracer_OnLog;
            Tracer.OnException += Tracer_OnException;
            // invoke Foo via Tracer to generate code tracing useful logs on Enter, [on Exit], [on Exception].
            // run time measurement is also logged on generated "on Exit" logs.
            Tracer.Invoke(Foo, funcFootprint: "Foo()");

            // invoke FooThatThrows.
            var i = 123;
            Tracer.Invoke(FooThatThrows, i, InvokeVerbosity.OnEnter,
                funcFootprint: string.Format("FooThatThrows({0})", i));
        }
        #endregion

        /// <summary>
        /// Result Evaluator sample method.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="runTime"></param>
        /// <param name="customMessage"></param>
        /// <returns></returns>
        private static ResultActionType resultEval(object result, TimeSpan? runTime, out string customMessage)
        {
            if (runTime.Value.TotalSeconds >= 5)
            {
                // log a failed warning msg if method took more than 4 seconds.
                customMessage = "Warning! Method took more than 4 secs to run.";
                return ResultActionType.Fail;
            }
            // log a passed msg if method ran faster than 5 seconds.
            customMessage = null;
            return ResultActionType.Pass;
        }


        private static void Tracer_OnException(Exception exc, string functionInfo)
        {
            var msg = string.Format("{0}, details: {1}", functionInfo, exc.ToString());
            Tracer_OnLog(LogLevels.Fatal, null, msg);

            // you can rethrow exc if needed.
            //throw exc;
        }
        private static void Tracer_OnLog(LogLevels logLevel, string[] category, string message)
        {
            // just print to console to simulate logging!
            string cat = "None";
            if (category != null)
                cat = string.Join(", ", category);
            var msg = string.Format("{0}: {1}: {2}: {3}", DateTime.Now, cat, logLevel, message);
            Console.WriteLine(msg);
        }

        static int Foo()
        {
            int a = 1 + 2;
            Tracer_OnLog(LogLevels.Information, null, "Inside Foo.");
            return a;
        }
        static int FooTakesTime(int delayTimeInSec = 5)
        {
            int a = 1 + 2;
            Tracer_OnLog(LogLevels.Information, null, string.Format("Sleeping for {0} secs in FooTakesTime.", delayTimeInSec));
            System.Threading.Thread.Sleep(delayTimeInSec * 1000);
            return a;
        }
        static int FooThatThrows(int arg1)
        {
            throw new Exception("Inside Foo That Throws!");
        }
    }
}
