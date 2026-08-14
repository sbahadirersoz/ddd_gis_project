using gis.ApplicationLayer.Common;
using gis.Domain.Contracts;
using gis.Domain.Services;
using gis.InfrastructureLayer.DB.Context;
using gis.InfrastructureLayer.Repositories;
using gis.InfrastructureLayer.Topology_Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.DependencyInjection;

public  static class DependencyInjector
{
    public static IServiceCollection InjectServicesFromInfrastructure(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(PointRepository))
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository") || c.Name.EndsWith("Service") || c.Name.EndsWith("Contract")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        
        services.AddScoped<POIDomainService>();
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddSingleton<GeometryFactory>(provider => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        services.AddSingleton<ITopologySuitePointContract, TopologySuitePointContractImpl>();
        return services;
    }

    public static IServiceCollection InjectDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>
        (opt => opt.UseNpgsql(configuration.GetConnectionString("Default"),
            x => x.UseNetTopologySuite()
        ));
        return services;
    }
}