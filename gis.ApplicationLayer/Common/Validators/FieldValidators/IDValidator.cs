using FluentValidation;
using gis.Domain.Common;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Common.Validators;

public class IDValidator:AbstractValidator<Guid>
{
    public IDValidator()
    {
        RuleFor(x => x).NotEmpty().NotEqual(Guid.Empty)
            .WithErrorCode(DomainErrors.EMPTY_ID_FORMAT.Code)
            .WithMessage(DomainErrors.EMPTY_ID_FORMAT.Desc);
        


    }
}