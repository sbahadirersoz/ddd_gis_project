using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.InfrastructureLayer.HelperMethods;

namespace gis.InfrastructureLayer.Topology_Services;

public class TopologySuitePointContractImpl : ITopologySuitePointContract
{
    public Result<string> CreateWktStringFromLonLat(Latitude latitude, Longitude longitude)
    {
        var result = CommonGeometryHelpers.FromLonLatCoordinatesToWKTString(latitude, longitude);
        return result.IsFailure ? Result<string>.Failure(result.Error) : Result<string>.Success(result.Value);
    }
    
}