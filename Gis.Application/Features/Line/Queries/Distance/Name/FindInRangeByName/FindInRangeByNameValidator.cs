using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindInRangeByName;

public class FindInRangeByNameValidator:AbstractValidator<FindInRangeByNameQuery>
{
    public FindInRangeByNameValidator()
    {
        RuleFor(x => x.DistanceInMeter).SetValidator(new RangeValidator());
        RuleFor(x => x.name).SetValidator(new NameValidator());
    }
}