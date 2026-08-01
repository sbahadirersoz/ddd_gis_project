using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Coord;

public record Longitude
{
    public double Value { get; }

    private Longitude(double value)
    {
        Value = value;
    }

    public static Result<Longitude> Create(double value)
    {
        var validate = Validate(value);
        return validate.IsFailure
            ? Result<Longitude>.Failure(validate.Error)
            : Result<Longitude>.Success(new Longitude(value));
    }

    private static Result Validate(double longitude)
    {
        return longitude switch
        {
            < HardCodedPropertities.CoordinatePropertities.MinLongitude
                => Result.Failure(DomainErrors.POIErrors.Coordinate.LONGITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR),
            > HardCodedPropertities.CoordinatePropertities.MaxLongitude
                => Result.Failure(DomainErrors.POIErrors.Coordinate.LONGITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR),
            _ => Result.Success()
        };
    }
}
