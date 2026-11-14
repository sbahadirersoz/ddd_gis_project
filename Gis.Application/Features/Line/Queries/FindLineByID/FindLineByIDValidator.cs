using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.FindLineByID;

public class FindLineByIDValidator:AbstractValidator<FindLineByIDQuery>
{
    public FindLineByIDValidator()
    {
        RuleFor(x=>x.id).SetValidator(new IDValidator());
    }
}