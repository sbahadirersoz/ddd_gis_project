
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
    

    
    
}