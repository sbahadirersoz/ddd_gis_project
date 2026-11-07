using FluentValidation;

using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public class FindInRangePoisByWktValidator:AbstractValidator<FindInRangePoisByWktQuery>
{
    public FindInRangePoisByWktValidator()
    {
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
    }
}