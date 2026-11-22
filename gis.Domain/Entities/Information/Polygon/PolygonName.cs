using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Information.Polygon;

public record PolygonName
{
    private PolygonName(string value)
    {
        Value = value;
    }

    public string Value { get; set; }


    public static Result<PolygonName> CreateFromString(string name)
    {
        var validate = validatePolyName(name);
        return (validate.IsFailure)
                ? Result<PolygonName>.Failure(validate.Error)
                : Result<PolygonName>.Success(new PolygonName(name));
    }


    private static Result validatePolyName(string value)
    {
        var isNullOrWhiteSpaced = valueIsNullOrWhiteSpaced(value);
        var validatePointNameLen = validatePolyNameLen(value);

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

    private static Result validatePolyNameLen(string value)
    {
        return value.Length switch
        {
            >= HardCodedPropertities.NameMaxLength
                => Result.Failure(DomainErrors.Polygons.LENGTH_REACHED_MAX_VALUE),
            <= HardCodedPropertities.NameMinLength
                => Result.Failure(DomainErrors.Polygons.LENGTH_UNDER_MIN_VALUE),
            _ => Result.Success()
        };
    }

    private static Result valueIsNullOrWhiteSpaced(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(DomainErrors.Polygons.BAD_CREDENTIALS_FOR_NAME);
        }

        if (value.Contains(" "))
        {
            return Result.Failure(DomainErrors.Polygons.BAD_CREDENTIALS_FOR_NAME);
        }

        return Result.Success();
    }
}