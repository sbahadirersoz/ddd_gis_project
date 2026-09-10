using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public class FindMostClosesCountByWKTValidator
    :AbstractValidator<FindMostClosesLineCountByWKTQuery>
{
    public FindMostClosesCountByWKTValidator()
    {
        RuleFor(x => x.wkt).SetValidator(new WKTValidator());
        RuleFor(x => x.count).SetValidator(new CountValidator());
    }
}