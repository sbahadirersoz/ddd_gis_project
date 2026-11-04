using Gis.Application.Common;
using gis.Domain.BusinessRules.PolygonRules;
using gis.Domain.BusinessRules.PolygonRules.Abstract;
using gis.Domain.Contracts;
using gis.Domain.Services;
using Gis.Infrastructure.DB.Context;
using Gis.Infrastructure.Repositories;
using Gis.Infrastructure.Topology_Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Gis.Infrastructure.DependencyInjection;

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
        services.AddScoped<IPolygonBusinessRulesFactory, PolygonBusinessRulesFactory>();
        services.AddScoped<IPolygonDomainServiceContract, PolygonDomainServiceContract>();
        services.AddScoped<ITopologySuitePolygonValidationContract, TopologySuitePolygonValidationContractImpl>();
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