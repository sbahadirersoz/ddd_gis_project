using FluentValidation;

using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

public class FindMostClosesCountByNameQueryValidator:AbstractValidator<FindMostClosesCountByNameQuery>
{
    public FindMostClosesCountByNameQueryValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
        RuleFor(x => x.name).SetValidator(new NameValidator());
    }
}