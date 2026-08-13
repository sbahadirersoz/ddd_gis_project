using gis.Domain.Contracts;
using gis.InfrastructureLayer.Topology_Services;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.DependencyInjection;

public  static class DependencyInjector
{
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        services.AddSingleton<GeometryFactory>(provider => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        services.AddSingleton<ITopologySuitePointContract, TopologySuitePointContractImpl>();
        return services;
    }
}