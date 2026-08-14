using gis.ApplicationLayer.Features.Poi.Commands.Create;
using Microsoft.Extensions.DependencyInjection;

namespace gis.ApplicationLayer.DependencyInjection;

public static class DependencyInjector
{

    public static IServiceCollection MediatRInjection(this IServiceCollection services)
    {
        ///MediatR  Injection
        services.AddMediatR
        (cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjector).Assembly)
        );
        return services;
    }
    public static IServiceCollection ServiceInjection(this IServiceCollection services)
    {
        
        services.AddScoped(typeof(CreatePoiCommandHandler));
        return services;
    }
    
    public static IServiceCollection FluentValidationInjection(this IServiceCollection services)
    {
        
        ///MapperInjection
        return services;
    }
    
    
}