using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Queries.FindLineByID;

public class FindLineByIDValidator:AbstractValidator<Guid>
{
    public FindLineByIDValidator()
    {
        RuleFor(x=>x).SetValidator(new IDValidator());
    }
}