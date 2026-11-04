using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Coord;

public record Latitude
{
    private Latitude(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Result<Latitude> Create(double value)
    {
        var validate = Validate(value);
        return validate.IsFailure ? Result<Latitude>.Failure(validate.Error) : Result<Latitude>.Success(new Latitude(value));
    }
    private static Result Validate(double latitude)
    {
        return latitude switch
        {
            < HardCodedPropertities.CoordinatePropertities.MinLatitude => Result.Failure(DomainErrors.POIErrors.Coordinate.LATITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR),
            > HardCodedPropertities.CoordinatePropertities.MaxLatitude => Result.Failure(DomainErrors.POIErrors.Coordinate.LATITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR),
            _ => Result.Success()
        };
    }

};