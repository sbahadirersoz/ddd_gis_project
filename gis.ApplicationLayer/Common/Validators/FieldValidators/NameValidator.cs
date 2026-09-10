using FluentValidation;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Common.Validators;

public class NameValidator:AbstractValidator<string>
{
    public NameValidator()
    {

        RuleFor(x => x).NotEmpty().Length
                (HardCodedPropertities.NameMinLength, HardCodedPropertities.NameMaxLength)
            .WithErrorCode(DomainErrors.BAD_CREDENTIALS_FOR_NAME.Code)
            .WithMessage(DomainErrors.BAD_CREDENTIALS_FOR_NAME.Desc);
    }
}