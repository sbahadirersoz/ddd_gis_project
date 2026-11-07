using FluentValidation;

using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.FindInRangePoisByName;

public class FindInRangePoisByNameQueryValidator:AbstractValidator<FindInRangePoisByNameQuery>
{
    public FindInRangePoisByNameQueryValidator()
    {
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
        RuleFor(x => x.name).SetValidator(new NameValidator());
    }
}