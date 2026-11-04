using FluentValidation;
using gis.Domain.ResultPattern.Errors;

namespace Gis.Application.Common.Validators;

public class WKTValidator:AbstractValidator<string>
{
    public WKTValidator()
    {
        RuleFor(x => x).NotEmpty().Must(x =>
            (
                x.StartsWith("POINT", StringComparison.OrdinalIgnoreCase) &&
                x.StartsWith("LINESTRING ", StringComparison.OrdinalIgnoreCase) &&
                x.StartsWith("POLYGON", StringComparison.OrdinalIgnoreCase)
            )
        ).WithErrorCode(DomainErrors.WKT.INVALID_WKT_FORMAT.Code)
        .WithMessage(DomainErrors.WKT.INVALID_WKT_FORMAT.Desc);
        
    }
}