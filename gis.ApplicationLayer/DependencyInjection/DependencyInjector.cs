
using FluentValidation;
using gis.ApplicationLayer.Pipelines.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace gis.ApplicationLayer.DependencyInjection;

public static class DependencyInjector
{

    public static IServiceCollection MediatRInjection(this IServiceCollection services)
    {
        ///MediatR  Injection
        services.AddMediatR
        (cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjector).Assembly);
            cfg.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        }
        );
        return services;
    }

    public static IServiceCollection FluentValidationInjection(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjector).Assembly);
        return services;
    }
}