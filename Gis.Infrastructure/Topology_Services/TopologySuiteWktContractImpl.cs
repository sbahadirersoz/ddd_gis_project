using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using Gis.Infrastructure.HelperMethods;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Gis.Infrastructure.Topology_Services;

public class TopologySuiteWktContractImpl : ITopologySuiteWKTContract
{
    public Result<string> CreateWktStringFromLonLat(Latitude latitude, Longitude longitude)
    {
        var result = CommonGeometryHelpers.FromLonLatCoordinatesToWKTString( longitude,latitude);
        return result.IsFailure ? Result<string>.Failure(result.Error) : Result<string>.Success(result.Value);
    }
    public Result<string> CreateWktStringFromCoordList(List<CoordinateValueObject> coords)
    {
        var fromCoordinateVoListToLineStringWkt = CommonGeometryHelpers.FromCoordinateVOListToLineStringWKT(coords);
        return (fromCoordinateVoListToLineStringWkt != null)
            ? Result<string>.Success(fromCoordinateVoListToLineStringWkt)
            : Result<string>.Failure(DomainErrors.LineErrors.WKT.INVALID_WKT_FORMAT);
    }

    public Result<WellKnownText> ParsePointWkt(string wkt)
    {
        var created = WellKnownText.Create(wkt);
        if (created.IsFailure) return created;

        Geometry geometry;
        try
        {
            geometry = new WKTReader().Read(created.Value.Value);
        }
        catch (ParseException)
        {
            return Result<WellKnownText>.Failure(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);
        }

        if (geometry is not Point { IsEmpty: false } point || !double.IsFinite(point.X) || !double.IsFinite(point.Y))
            return Result<WellKnownText>.Failure(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);

        var longitude = Longitude.Create(point.X);
        if (longitude.IsFailure) return Result<WellKnownText>.Failure(longitude.Error);

        var latitude = Latitude.Create(point.Y);
        if (latitude.IsFailure) return Result<WellKnownText>.Failure(latitude.Error);

        return created;
    }
}