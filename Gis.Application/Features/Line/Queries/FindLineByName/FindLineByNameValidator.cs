using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.FindLineByName;

public class FindLineByNameValidator:AbstractValidator<string>
{
    public FindLineByNameValidator()
    {
        RuleFor(x => x).SetValidator(new NameValidator());
    }
}