using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public class FindMostClosesCountByNameValidator:AbstractValidator<FindMostClosesCountByNameQuery>
{
    public FindMostClosesCountByNameValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
        RuleFor(x => x.Name).SetValidator(new NameValidator());
    }
}