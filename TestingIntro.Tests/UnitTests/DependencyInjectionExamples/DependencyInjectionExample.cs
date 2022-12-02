using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

/*
 * Tutorial: Use dependency injection in .NET
 * https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage
 * 
 * AddTransient, AddScoped and AddSingleton Services Differences
 * https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences
 * 
  * AddSingleton() 
  * Creates a single instance of the service when it is first requested and reuses that same instance in all the places where that service is needed.
  * 
  * AddScoped() 
  * In a scoped service, with every HTTP request, we get a new instance. However, within the same HTTP request, if the service is required in multiple places, like in the view and in the controller, then the same instance is provided for the entire scope of that HTTP request. But every new HTTP request will get a new instance of the service.
  * 
  * AddTransient()
  * With a transient service, a new instance is provided every time a service instance is requested whether it is in the scope of the same HTTP request or across different HTTP requests.* 
  * 
  * Injecting service with different lifetimes into another
  * * Never inject Scoped & Transient services into Singleton service. (This effectively converts the transient or scoped service into the singleton.)
  * * Never inject Transient services into scoped service ( This converts the transient service into the scoped.)
 */

namespace TestingIntro.Tests.UnitTests.DependencyInjectionExamples
{
    public class DependencyInjectionExample
    {
        public DependencyInjectionExample(ITestOutputHelper testOutputHelper)
        {
            TestUtility.SetTestOutputLogger(testOutputHelper);
        }

        [Theory]
        [InlineData(OperationServiceType.Transient)]
        [InlineData(OperationServiceType.Scoped)]
        [InlineData(OperationServiceType.Singleton)]
        [Trait("Category", "T")]
        [Conditional("DEBUG")]
        public void DependencyInjectionExampleTest(OperationServiceType operationServiceType)
        {
            using var host = Host.CreateDefaultBuilder()
                                 .ConfigureServices((_, services) => services.ConfigureOurServices(operationServiceType))
                                 .UseSerilog()
                                 .Build();

            ExemplifyScoping(host.Services, "Scope 1");
            Log.Information("...");
            ExemplifyScoping(host.Services, "Scope 2");
            Log.Information("");

            _ = host.RunAsync();
        }

        void ExemplifyScoping(IServiceProvider services, string scope)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            OperationService logger = provider.GetRequiredService<OperationService>();
            logger.LogOperations($"{scope}-Call 1 .GetRequiredService<OperationLogger>()");
            Log.Information("...");

            logger = provider.GetRequiredService<OperationService>();
            logger.LogOperations($"{scope}-Call 2 .GetRequiredService<OperationLogger>()");
        }
    }

    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection ConfigureOurServices(this IServiceCollection services, OperationServiceType operationServiceType)
        {
            services.AddTransient<ITransientOperation, DefaultOperation>();
            services.AddScoped<IScopedOperation, DefaultOperation>();
            services.AddSingleton<ISingletonOperation, DefaultOperation>();
            
            switch (operationServiceType)
            {
                case OperationServiceType.Transient: services.AddTransient<OperationService>(); break;
                case OperationServiceType.Scoped: services.AddScoped<OperationService>(); break;
                case OperationServiceType.Singleton: services.AddSingleton<OperationService>(); break;
            }

            return services;
        }
    }

    public enum OperationServiceType 
    {
        Transient,
        Scoped,
        Singleton
    }

    public class DefaultOperation : ITransientOperation, IScopedOperation, ISingletonOperation
    {
        private Guid _guid;
        private readonly ILogger<DefaultOperation> _logger;

        public DefaultOperation(ILogger<DefaultOperation> logger)
        {
            _logger = logger;
            _guid = Guid.NewGuid();

            _logger.LogInformation($"DefaultOperaton constructor: GetUniqueId() = {GetUniqueId()}");
        }
        
        public Guid OperationId => _guid;

        public string GetUniqueId()
        {
            // return $"OperationId = {_guid}";
            return $"OperationId = {_guid}, DateTime.Now.Ticks = {DateTime.Now.Ticks}";
        }
    }

    public interface IOperation { Guid OperationId { get; } string GetUniqueId(); }
    public interface ISingletonOperation : IOperation { }
    public interface ITransientOperation : IOperation { }
    public interface IScopedOperation : IOperation { }

    public class OperationService
    {
        private readonly ITransientOperation _transientOperation;
        private readonly IScopedOperation _scopedOperation;
        private readonly ISingletonOperation _singletonOperation;
        private readonly DefaultOperation _defaultOperation;
        private readonly ILogger<OperationService> _logger;

        public OperationService(ITransientOperation transientOperation,
                               IScopedOperation scopedOperation,
                               ISingletonOperation singletonOperation,
                               ILogger<OperationService> logger)
        {
            _transientOperation = transientOperation;
            _scopedOperation = scopedOperation;
            _singletonOperation = singletonOperation;
            _logger = logger;
        }

        public void LogOperations(string scope)
        {
            _logger.LogInformation(LogOperation(_transientOperation, scope, "OperationId should always different"));
            _logger.LogInformation(LogOperation(_scopedOperation, scope, "OperationId should only changes with new scope"));
            _logger.LogInformation(LogOperation(_singletonOperation, scope, "OperationId is always the same"));
        }
        private static string LogOperation<T>(T operation, string scope, string message)
            where T : IOperation
        {
            return $"{scope}: {typeof(T).Name,-27} [ {operation.GetUniqueId()}...{message,-23} ]";
        }
    }
}