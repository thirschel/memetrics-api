using MeMetrics.Api.Middleware.Logging;
using MeMetrics.Infrastructure;
using Lamar;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeMetrics.Api.Configurations.Extensions
{
    public static class DependencyInjectionConfigurationExtensions
    {
        internal static void AddDependencyInjection(this ServiceRegistry services, IConfiguration configuration)
        {
            // Map the environment variables to an object that represents them
            ((IServiceCollection)services).Configure<EnvironmentConfiguration>(configuration);

            // https://jasperfx.github.io/lamar/documentation/ioc/registration/auto-registration-and-conventions/
            services.Scan(_ =>
            {
                _.TheCallingAssembly();
                _.Assembly("MeMetrics.Application");
                _.Assembly("MeMetrics.Infrastructure");
                _.AddAllTypesOf<IValidator>();
                _.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                _.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                _.WithDefaultConventions();
                _.LookForRegistries();
            });
            services.AddTransient<IMediator, Mediator>();
            services.For<ServiceFactory>().Use(ctx => ctx.GetInstance);
            services.AddSingleton(LoggingServiceFactory.LoggingService);
        }
    }
}
