using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Information;

public record PointName
{
    public string Value { get; init; }

    private PointName(string value)
    {
        Value = value;
    }

    public static Result<PointName> FromString(string value)
    {
        var result = validatePointName(value);
        return result.IsSuccess
            ? Result<PointName>.Success(new PointName(value))
            : Result<PointName>.Failure(result.Error);
    }

    private static Result validatePointName(string value)
    {
        var isNullOrWhiteSpaced = valueIsNullOrWhiteSpaced(value);
        var validatePointNameLen = PointName.validatePointNameLen(value);
        
        if (isNullOrWhiteSpaced.IsFailure)
        {
            return Result.Failure(isNullOrWhiteSpaced.Error);
        }
        if (validatePointNameLen.IsFailure)
        {
            return Result.Failure(validatePointNameLen.Error);
        }


        return  Result.Success();
    }

    private static Result validatePointNameLen(string value)
    {
        return value.Length switch
        {
            >= HardCodedPropertities.PointNamePropertities.MaxLength
                => Result.Failure(DomainErrors.POIErrors.PointName.LENGTH_REACHED_MAX_VALUE),
            <= HardCodedPropertities.PointNamePropertities.MinLength
                => Result.Failure(DomainErrors.POIErrors.PointName.LENGTH_UNDER_MIN_VALUE),
            _ => Result.Success()
        };
    }

    private static Result valueIsNullOrWhiteSpaced(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(DomainErrors.POIErrors.PointName.BAD_CREDENTIALS_FOR_POINT_NAME);
        }
        if (value.Contains(" "))
        {
            return Result.Failure(DomainErrors.POIErrors.PointName.BAD_CREDENTIALS_FOR_POINT_NAME);
            
        }

        return Result.Success();
    }
}