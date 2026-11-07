using FluentValidation;

using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.FindPoisInGivenRange;

public class FindMostClosesCountByIdQueryValidator:AbstractValidator<FindMostClosesCountByIdQuery>

{
    public FindMostClosesCountByIdQueryValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
    }
}