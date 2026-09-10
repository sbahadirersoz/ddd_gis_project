using gis.Domain.ResultPattern;

namespace gis.Domain.Entities.Information.Polygon;

public record PolygonName
{
    private PolygonName(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
    
    
    
    public static Result<PolygonName> CreateFromString(string name)  =>  Result<PolygonName>.Success(new PolygonName(name));
}