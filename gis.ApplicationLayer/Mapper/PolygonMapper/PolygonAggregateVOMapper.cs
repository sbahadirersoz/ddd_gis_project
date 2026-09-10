using gis.ApplicationLayer.Dtos;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;

namespace gis.ApplicationLayer.Mapper.PolygonMapper;

public static class PolygonAggregateVOMapper
{
    public static Result<PolygonName> CreatePolyNameFromString(string polygonName)
    => PolygonName.CreateFromString(polygonName);
    public static Result<PolygonDescription> CreatePolyDescFromString(string polygonDesc)
    => PolygonDescription.CreateFromString(polygonDesc);

    public static Result<PolygonShell> CoordinateDtoListToPolygonShell(List<CoordinateDto> coords, WellKnownText wkt)
    {
        var list = DtoToValueObjects(coords);
        return PolygonShell.Create(list,wkt);
    }

    private  static Result<CoordinateValueObject> primitivesToCoordinates(double latitude, double longitude)
    {
        var LatResult = Latitude.Create(latitude);
        var LonResult = Longitude.Create(longitude);
        return CoordinateValueObject.ForPolygonCoordCreation(LatResult.Value, LonResult.Value);
    }
    public static List<CoordinateValueObject> DtoToValueObjects(List<CoordinateDto> dto) =>
        dto.Select(x => primitivesToCoordinates(x.Latitude, x.Longitude).Value).ToList();



}