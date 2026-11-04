using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindClosestByName;

public class FindClosestByNameValidator:AbstractValidator<FindClosestLineByNameQuery>
{
    public FindClosestByNameValidator()
    {
        RuleFor(x => x.name).SetValidator(new NameValidator());
    }
}