using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public class FindClosestByWKTValidator:AbstractValidator<FindClosestLineByWKTQuery>
{
    public FindClosestByWKTValidator()
    {
        RuleFor(x => x.wkt).SetValidator(new WKTValidator());
    }
}