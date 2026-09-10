using gis.Domain.Entities.Coord;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.InfrastructureLayer.Topology_Services;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace gis.InfrastructureLayer.HelperMethods;

public static class CommonGeometryHelpers
{
    private static readonly GeometryFactory _geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    private static readonly WKTReader WktReader = new(_geometryFactory);

    
    
    
    
public static Result<string> FromLonLatCoordinatesToWKTString(Latitude latitude, Longitude longitude)
    {
        var fromLatLonToCoordinate = CoordinateFactory.FromLonLatToPoint(latitude, longitude);
        return (fromLatLonToCoordinate.IsFailure)
            ? Result<string>.Failure(fromLatLonToCoordinate.Error)
            : Result<string>.Success(WKTWriter.ToPoint(fromLatLonToCoordinate.Value.Coordinate));
    }

    public static string FromCoordinateVOListToLineStringWKT(List<CoordinateValueObject> coords)
    {
        var CoordinateList = CoordinateFactory.FromCoordListToCoordinates(coords);
        var LineString = CoordinateFactory.CoordinatesToLineString(CoordinateList);
        return WKTWriter.ToLineString(LineString.Coordinates);
    }
    
    

    public static Point WktToPoint(WellKnownText wkt)
    {
        var geometry = WktReader.Read(wkt.Value);
        return (Point)geometry;
    }
}