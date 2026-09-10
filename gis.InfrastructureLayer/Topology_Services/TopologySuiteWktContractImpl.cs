using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.InfrastructureLayer.HelperMethods;

namespace gis.InfrastructureLayer.Topology_Services;

public class TopologySuiteWktContractImpl : ITopologySuiteWKTContract
{
    public Result<string> CreateWktStringFromLonLat(Latitude latitude, Longitude longitude)
    {
        var result = CommonGeometryHelpers.FromLonLatCoordinatesToWKTString(latitude, longitude);
        return result.IsFailure ? Result<string>.Failure(result.Error) : Result<string>.Success(result.Value);
    }
    public Result<string> CreateWktStringFromCoordList(List<CoordinateValueObject> coords)
    {
        var fromCoordinateVoListToLineStringWkt = CommonGeometryHelpers.FromCoordinateVOListToLineStringWKT(coords);
        return (fromCoordinateVoListToLineStringWkt != null)
            ? Result<string>.Success(fromCoordinateVoListToLineStringWkt)
            : Result<string>.Failure(DomainErrors.LineErrors.WKT.INVALID_WKT_FORMAT);
    }
}