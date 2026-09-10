using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindInRangeByWKT;

public class FindInRangeLineByWKTValidator:AbstractValidator<FindInRangeLineByWKTQuery>
{
    public FindInRangeLineByWKTValidator()
    {
        RuleFor(x => x.wkt).SetValidator(new WKTValidator());
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
    }
}