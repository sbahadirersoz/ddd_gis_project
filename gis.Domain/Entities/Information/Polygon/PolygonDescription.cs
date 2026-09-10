using gis.Domain.ResultPattern;

namespace gis.Domain.Entities.Information.Polygon;

public record PolygonDescription

{
    public string? Value { get; init; }

    private PolygonDescription(string? value)
    {
        Value = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
    }

    public static Result<PolygonDescription> CreateFromString(string? desc)
        => Result<PolygonDescription>.Success(new PolygonDescription(desc));
}