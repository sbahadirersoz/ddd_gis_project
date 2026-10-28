using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public class FindMostClosesCountByWKTValidator
    :AbstractValidator<FindMostClosesLineCountByWKTQuery>
{
    public FindMostClosesCountByWKTValidator()
    {
        RuleFor(x => x.wkt).SetValidator(new WKTValidator());
        RuleFor(x => x.count).SetValidator(new CountValidator());
    }
}