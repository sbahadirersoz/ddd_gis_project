using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.HelperMethods;

public static class CoordinateFactory
{
    private static readonly  GeometryFactory _geometryFactory =NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    public static Result<Point> FromLonLatToPoint(Latitude latitude, Longitude longitude)
    {
        var point = new Point(longitude.Value,latitude.Value);
        return !point.IsValid
            ? Result<Point>.Failure(DomainErrors.POIErrors.Coordinate.BAD_CREDENTIALS_FOR_COORDINATES)
            : Result<Point>.Success(point);
    }
    public static Result<Coordinate> FromLonLatToCoordinate(Latitude latitude, Longitude longitude)
    {
        var point = new Coordinate(longitude.Value,latitude.Value);
        return !point.IsValid
            ? Result<Coordinate>.Failure(DomainErrors.POIErrors.Coordinate.BAD_CREDENTIALS_FOR_COORDINATES)
            : Result<Coordinate>.Success(point);
    }
    
    
    
}