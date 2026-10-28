using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindInRangeByWKT;

public class FindInRangeLineByWKTValidator:AbstractValidator<FindInRangeLineByWKTQuery>
{
    public FindInRangeLineByWKTValidator()
    {
        RuleFor(x => x.wkt).SetValidator(new WKTValidator());
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
    }
}