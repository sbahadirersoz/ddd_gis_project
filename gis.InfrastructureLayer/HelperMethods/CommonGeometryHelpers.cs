using gis.Domain.Entities.Coord;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using gis.InfrastructureLayer.Topology_Services;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace gis.InfrastructureLayer.HelperMethods;

public static class CommonGeometryHelpers
{
    private static readonly GeometryFactory _geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    private static readonly TopologySuitePointContractImpl _contract;
    
    
    /// <summary>
    /// Reponun yazılması için b
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    public static Result<string> FromLonLatCoordinatesToWKTString(Latitude latitude, Longitude longitude)
    {
        var fromLatLonToCoordinate = CoordinateFactory.FromLonLatToPoint(latitude, longitude);
        return (fromLatLonToCoordinate.IsFailure)
            ? Result<string>.Failure(fromLatLonToCoordinate.Error)
            : Result<string>.Success(WKTWriter.ToPoint(fromLatLonToCoordinate.Value.Coordinate));
    }

    public static Point WktToPoint(WellKnownText wkt)
    {
        var wktReader = new WKTReader();
        var geometry = wktReader.Read(wkt.Value);
        return (Point)geometry;
    }

    public static Point LonLatToPoint(Longitude longitude, Latitude latitude)=> new (longitude.Value,latitude.Value);

    
}