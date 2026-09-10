using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindInRangeByName;

public class FindInRangeByNameValidator:AbstractValidator<FindInRangeByNameQuery>
{
    public FindInRangeByNameValidator()
    {
        RuleFor(x => x.DistanceInMeter).SetValidator(new RangeValidator());
        RuleFor(x => x.name).SetValidator(new NameValidator());
    }
}