using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using static System.String;

namespace gis.Domain.Entities.Information.Line;

public record LineDescription
{
    public string? Value { get; init; }

    private LineDescription(string? value) 
    {
        Value = value ?? null;
    }
    public static  Result<LineDescription>  FromString(string? value)
    {
        if (IsNullOrWhiteSpace(value))
            return Result<LineDescription>.Success(new LineDescription(Empty));
        var pointDescValidation = validatePointDesc(value);
        return pointDescValidation.IsFailure ? Result<LineDescription>.Failure(pointDescValidation.Error) : Result<LineDescription>.Success(new LineDescription(value));
    }

    private static Result validatePointDesc(string value)
    {
        var length = validateLength(value);
        return length.IsFailure ? Result.Failure(length.Error) : Result.Success();
    }
    private static  Result validateLength(string value)
    {
        return value.Length > HardCodedPropertities.DescriptionMaxLength ? Result.Failure(DomainErrors.POIErrors.PointDesc.LENGTH_REACHED_MAX_VALUE) : Result.Success();
    }
};