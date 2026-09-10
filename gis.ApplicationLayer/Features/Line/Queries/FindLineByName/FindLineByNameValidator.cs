using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Queries.FindLineByName;

public class FindLineByNameValidator:AbstractValidator<string>
{
    public FindLineByNameValidator()
    {
        RuleFor(x => x).SetValidator(new NameValidator());
    }
}