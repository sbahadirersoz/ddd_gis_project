using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public class FindClosestByWKTValidator:AbstractValidator<FindClosestLineByWKTQuery>
{
    public FindClosestByWKTValidator()
    {
        RuleFor(x => x.wkt).SetValidator(new WKTValidator());
    }
}