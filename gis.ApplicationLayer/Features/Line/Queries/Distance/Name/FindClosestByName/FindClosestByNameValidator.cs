using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindClosestByName;

public class FindClosestByNameValidator:AbstractValidator<FindClosestLineByNameQuery>
{
    public FindClosestByNameValidator()
    {
        RuleFor(x => x.name).SetValidator(new NameValidator());
    }
}