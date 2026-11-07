using FluentValidation;

using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.Id.FindInRangePoisById;

public class FindInRangePoisByIdQueryValidator:AbstractValidator<FindInRangePoisByIdQuery>
{
    public FindInRangePoisByIdQueryValidator()
    {
        RuleFor(x => x.range).SetValidator(new RangeValidator());
    }
}