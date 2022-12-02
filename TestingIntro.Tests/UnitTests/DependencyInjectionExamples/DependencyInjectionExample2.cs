using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using TestingIntro.Tests.UnitTests.DependencyInjectionExamples;
using Xunit;
using Xunit.Abstractions;

/*
 * Dependency injection in .NET
 * https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
 * 
 * Background tasks with hosted services in ASP.NET Core
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio
 * 
 * Use ASP.NET Core hosted services to run a background task
 * https://www.roundthecode.com/dotnet/use-asp-net-core-hosted-services-run-background-task
 */

namespace TestingIntro.Tests.UnitTests.DependencyInjectionExamples2
{
    public class DependencyInjectionExample2
    {
        public DependencyInjectionExample2(ITestOutputHelper testOutputHelper)
        {
            TestUtility.SetTestOutputLogger(testOutputHelper, false);
        }

        [Fact]
        [Trait("Category", "T")]
        [Conditional("DEBUG")]
        public void DependencyInjectionExampleTest()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Host.UseSerilog();
            builder.Services.ConfigureOurServices();

            using var host = builder.Build();
            var exampleService = ActivatorUtilities.CreateInstance<ExampleService>(host.Services);
            exampleService.Test("This is only test");

            host.RunAsync()
                .GetAwaiter();
        }

        [Fact]
        [Trait("Category", "T")]
        [Conditional("DEBUG")]
        public void DependencyInjectionExampleTest2()
        {
            var builder = Host.CreateDefaultBuilder()
                              .ConfigureServices((_, services) => services.ConfigureOurServices());

            using var host = builder.UseSerilog()
                                    .Build();

            //var exampleService = host.Services.GetRequiredService<IExampleService>();
            //exampleService.Test("This is only test");

            Thread.Sleep(10000);
            _ = host.RunAsync();
        }
    }

    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection ConfigureOurServices(this IServiceCollection services)
        {
            return services.AddHostedService<MyHostedService>()
                           .AddHostedService<Worker>()
                           .AddScoped<IMessageWriter, LoggingMessageWriter>()
                           .AddTransient<IExampleService, ExampleService>();
        }
    }

    public interface IMessageWriter { void Write(string message); }

    public class LoggingMessageWriter : IMessageWriter
    {
        private readonly ILogger<LoggingMessageWriter> _logger;

        public LoggingMessageWriter(ILogger<LoggingMessageWriter> logger)
        {
            _logger = logger;
        }

        public void Write(string message)
        {
            _logger.LogInformation("Info: {Msg}", message);
        }
    }

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var exampleService = scope.ServiceProvider.GetRequiredService<IExampleService>();
                exampleService.Test("This is only test");

                _logger.LogInformation("Worker running at1: {time}", DateTimeOffset.Now);
                await Task.Delay(100, stoppingToken);
            }
        }
    }

    public class MyHostedService : IHostedService
    {

        private readonly ILogger<Worker> _logger;

        public MyHostedService(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at3: {time}", DateTimeOffset.Now);
                    await Task.Delay(new TimeSpan(0, 0, 5)); // 5 second delay
                    _logger.LogInformation("Worker running at4: {time}", DateTimeOffset.Now);
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public interface IExampleService { void Test(string input); }

    public class ExampleService : IExampleService
    {
        private readonly ILogger<ExampleService> _logger;
        public ExampleService(ILogger<ExampleService> logger)
        {
            _logger = logger;
        }

        public void Test(string input)
        {
            _logger.LogInformation(input);
        }
    }
}