using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.FindLineByID;

public class FindLineByIDValidator:AbstractValidator<Guid>
{
    public FindLineByIDValidator()
    {
        RuleFor(x=>x).SetValidator(new IDValidator());
    }
}