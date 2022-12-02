using Serilog;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TestingIntro.Tests.UnitTests.DependencyInjectionExamples
{
    public class TestUtility
    {
        public static ILogger GetConsoleLogger()
        {
            // create the logger and setup your sinks, filters and properties
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }

        private static ILogger GetTestOutputLogger(ITestOutputHelper testOutputHelper, bool shortOutput)
        {
            string outputTemplate = shortOutput ? "{Message:lj}{NewLine}{Exception}" : "[{Timestamp:HH:mm:ss.ffffff} {Level:u3}] {Message:lj}{NewLine}{Exception}";

            return new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Debug(Serilog.Events.LogEventLevel.Verbose, outputTemplate)
                            .WriteTo.TestOutput(testOutputHelper, Serilog.Events.LogEventLevel.Verbose, outputTemplate)
                            .CreateLogger();
        }

        public static void SetTestOutputLogger(ITestOutputHelper testOutputHelper, bool shortOutput = true)
        {
            Log.Logger = GetTestOutputLogger(testOutputHelper, shortOutput);
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg)); Serilog.Debugging.SelfLog.Enable(Console.Error);
        }
    }
}
