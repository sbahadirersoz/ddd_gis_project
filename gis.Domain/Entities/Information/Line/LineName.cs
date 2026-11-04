using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Information.Line;

public record LineName
{

    public string Value { get; init; }

    private LineName(string value)
    {
        Value = value;
    }

    public static Result<LineName> FromString(string value)
    {
        var result = validateLineName(value);
        return result.IsSuccess
            ? Result<LineName>.Success(new LineName(value))
            : Result<LineName>.Failure(result.Error);
    }

    private static Result validateLineName(string value)
    {
        var isNullOrWhiteSpaced = valueIsNullOrWhiteSpaced(value);
        var validatePointNameLen = LineName.validateLineNameLen(value);

        if (isNullOrWhiteSpaced.IsFailure)
        {
            return Result.Failure(isNullOrWhiteSpaced.Error);
        }

        if (validatePointNameLen.IsFailure)
        {
            return Result.Failure(validatePointNameLen.Error);
        }


        return Result.Success();


    }
    
    private static Result validateLineNameLen(string value)
    {
        return value.Length switch
        {
            >= HardCodedPropertities.NameMaxLength
                => Result.Failure(DomainErrors.POIErrors.PointName.LENGTH_REACHED_MAX_VALUE),
            <= HardCodedPropertities.NameMinLength
                => Result.Failure(DomainErrors.POIErrors.PointName.LENGTH_UNDER_MIN_VALUE),
            _ => Result.Success()
        };
    }

    private static Result valueIsNullOrWhiteSpaced(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(DomainErrors.LineErrors.Name.BAD_CREDENTIALS_FOR_LINE_NAME);
        }
        if (value.Contains(" "))
        {
            return Result.Failure(DomainErrors.LineErrors.Name.BAD_CREDENTIALS_FOR_LINE_NAME);
            
        }

        return Result.Success();
    }

}