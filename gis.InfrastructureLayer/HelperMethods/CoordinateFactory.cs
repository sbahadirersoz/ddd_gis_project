using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.HelperMethods;

public static class CoordinateFactory
{
    private static readonly  GeometryFactory _geometryFactory =NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    public static Result<Coordinate> FromLatLonToCoordinate(Latitude latitude, Longitude longitude)
    {
        var coordinate = new Coordinate(latitude.Value,longitude.Value);
        return !coordinate.IsValid
            ? Result<Coordinate>.Failure(DomainErrors.POIErrors.Coordinate.BAD_CREDENTIALS_FOR_COORDINATES)
            : Result<Coordinate>.Success(coordinate);
    }
}