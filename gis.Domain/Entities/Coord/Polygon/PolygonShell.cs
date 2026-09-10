using gis.Domain.BusinessRules.PolygonRules;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;

namespace gis.Domain.Entities.Coord.Polygon;

public record PolygonShell
{
    public List<CoordinateValueObject> Coordinates { get; }
    public WellKnownText wkt { get; set; }
    private PolygonShell(List<CoordinateValueObject> coordinates,WellKnownText wkt)
    {
        Coordinates = coordinates;
        this.wkt = wkt;
    }

    public static Result<PolygonShell> Create(List<CoordinateValueObject> coordinates,WellKnownText wkt)
    {
        var unit = new PolygonShell(coordinates  ,wkt);
        return  Result<PolygonShell>.Success(unit);
    }
    

    
    

    
}