using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracing;
using Tracing.InvokeEngine;
using Tracing.MSEnterpriseLogging;

namespace SampleTracerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize MSEL static log writer as is used by the "MSEL hooks" in Tracing MSEnterpriseLogging library.
            Logger.SetLogWriter(new LogWriterFactory().Create(), false);

            Console.WriteLine("Running Enterprise Logging Tracer demo.");
            DemoEnterpriseTracer();
            Console.WriteLine("Running Tracer logging to Console demo.");
            DemoTracerToConsole();
            Console.WriteLine();
            Console.WriteLine("Press <Enter> key to Quit.");
            Console.ReadLine();
        }
        private static void DemoEnterpriseTracer()
        {
            EnterpriseTracer Tracer = new EnterpriseTracer(resultEvaluator: 
                (object result, TimeSpan? runTime, out string customMessage) => 
                {
                    customMessage = null;
                    return ResultActionType.Default;
                });
            Tracer.Invoke(Foo, funcFootprint: "Foo()");
        }

        private static void DemoTracerToConsole()
        {
            // hook up console writer/logger for this demo!
            var Tracer = new Tracing.Tracer();
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

        private static void Tracer_OnException(Exception exc, string functionInfo)
        {
            var msg = string.Format("{0}, details: {1}", functionInfo, exc.ToString());
            Tracer_OnLog(LogLevels.Fatal, msg);
        }
        private static void Tracer_OnLog(LogLevels logLevel, string message)
        {
            // just print to console to simulate logging!
            var msg = string.Format("{0}: {1}, message: {2}", DateTime.Now, logLevel, message);
            Console.WriteLine(msg);
        }

        static int Foo()
        {
            int a = 1 + 2;
            Tracer_OnLog(LogLevels.Information, "Inside Foo.");
            return a;
        }
        static int FooThatThrows(int arg1)
        {
            throw new Exception("Inside Foo That Throws!");
        }
    }
}
