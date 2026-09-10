using FluentValidation;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Common.Validators;

public class StatusValidator:AbstractValidator<int>
{
    public StatusValidator()
    {
        RuleFor(x => x).InclusiveBetween(0, 2)
            .NotNull()
            .WithErrorCode(DomainErrors.BAD_CREDENTIALS_FOR_STATUS.Code)
            .WithMessage(DomainErrors.BAD_CREDENTIALS_FOR_STATUS.Desc);
    }
}