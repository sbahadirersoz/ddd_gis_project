using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace gis.InfrastructureLayer.HelperMethods;

public static class CommonGeometryHelpers
{
    private static readonly GeometryFactory _geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    
    
    public static Result<string> FromLatLonCoordinatesToWKTString(Latitude latitude, Longitude longitude)
    {
        var fromLatLonToCoordinate = CoordinateFactory.FromLatLonToCoordinate(latitude, longitude);
        return (fromLatLonToCoordinate.IsFailure)
            ? Result<string>.Failure(fromLatLonToCoordinate.Error)
            : Result<string>.Success(WKTWriter.ToPoint(fromLatLonToCoordinate.Value));
    }
}