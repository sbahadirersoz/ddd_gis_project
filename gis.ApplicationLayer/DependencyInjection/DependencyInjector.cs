using Microsoft.Extensions.DependencyInjection;

namespace gis.ApplicationLayer.DependencyInjection;

public static class DependencyInjector
{

    public static IServiceCollection MediatRInjection(this ServiceCollection services)
    {
        
        ///MediatR  Injection
        return services;
    }
    public static IServiceCollection MapperInjection(this ServiceCollection services)
    {
        
        ///MapperInjection
        return services;
    }
    
    public static IServiceCollection FluentValidationInjection(this ServiceCollection services)
    {
        
        ///MapperInjection
        return services;
    }
    
    
}