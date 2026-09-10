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

    public static Point LonLatToPoint(Longitude longitude, Latitude latitude)=> new (longitude.Value,latitude.Value);
    public static LineString CoordinatesToLineString(List<Coordinate> coords) => new ([.. coords]);
        
    public static List<Coordinate> FromCoordListToCoordinates(List<CoordinateValueObject> coords)
    {
        return coords.Select(x => new Coordinate(x.Longitude.Value, x.Latitude.Value)).ToList();
    }
    
    
    

    public static Polygon CoordinatesToPolygon(List<CoordinateValueObject> list)
    {
        var fromCoordListToCoordinates = FromCoordListToCoordinates(list);
        var linearRing = _geometryFactory.CreateLinearRing(fromCoordListToCoordinates.ToArray());
        var poly = _geometryFactory.CreatePolygon(linearRing);
        return poly;
    }

    
    


}