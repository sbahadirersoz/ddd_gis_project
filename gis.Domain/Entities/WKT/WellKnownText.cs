using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.WKT;

public record WellKnownText

{
    public string Value { get; }

    private WellKnownText(string value)
    {
        Value = value;
    }

    public static Result<WellKnownText> Create(string value)
    {
        if (!value.StartsWith("POINT", StringComparison.OrdinalIgnoreCase) && 
            !value.StartsWith("POLYGON", StringComparison.OrdinalIgnoreCase))
        {
            return Result<WellKnownText>.Failure(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);
        }

        return Result<WellKnownText>.Success(new WellKnownText(value.ToUpper()));
    }
};