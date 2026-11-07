using FluentValidation;

using Gis.Application.Common.Validators.QueryValidators;

using gis.Domain.ResultPattern.Errors;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

public class FindMostClosesCountPoiByLonLatValidator:AbstractValidator<FindMostClosesCountPoiByLonLatQuery>
{
    public FindMostClosesCountPoiByLonLatValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
        RuleFor(x => x.lon).NotNull()
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Desc);
        RuleFor(x => x.lat).NotNull()
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Desc);
    }
}