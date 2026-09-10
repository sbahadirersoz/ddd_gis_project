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
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository") || c.Name.EndsWith("Service") || c.Name.EndsWith("Contract") || c.Name.EndsWith("Factory")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        
        services.AddScoped<POIDomainService>();
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<ILineDomainServiceContract, LineDomainServiceContract>();
        services.AddSingleton<GeometryFactory>(provider => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        services.AddSingleton<ITopologySuiteWKTContract, TopologySuiteWktContractImpl>();
        return services;
    }

    public static IServiceCollection InjectDB(this IServiceCollection services, IConfiguration configuration)
    {
        var envConn = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION") ?? Environment.GetEnvironmentVariable("CONNECTION_STRING");
        var connString = !string.IsNullOrWhiteSpace(envConn) ? envConn : configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(connString, x => x.UseNetTopologySuite())
        );
        return services;
    }
}