using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using static System.String;

namespace gis.Domain.Entities.Information;

public record PointDescription

{
    public string? Value { get; init; }

    private PointDescription(string? value)
    {
        Value = value ?? null;
    }
    public static  Result<PointDescription>  FromString(string? value)
    {
        if (IsNullOrWhiteSpace(value))
            return Result<PointDescription>.Success(new PointDescription(Empty));
        var pointDescValidation = validatePointDesc(value);
        return pointDescValidation.IsFailure ? Result<PointDescription>.Failure(pointDescValidation.Error) : Result<PointDescription>.Success(new PointDescription(value));

    }

    private static Result validatePointDesc(string value)
    {
        var length = validateLength(value);
        return length.IsFailure ? Result.Failure(length.Error) : Result.Success();
    }
    private static  Result validateLength(string value)
    {
        return value.Length > HardCodedPropertities.PointDescriptionPropertities.MaxLength ? Result.Failure(DomainErrors.POIErrors.PointDesc.LENGTH_REACHED_MAX_VALUE) : Result.Success();
    }
}