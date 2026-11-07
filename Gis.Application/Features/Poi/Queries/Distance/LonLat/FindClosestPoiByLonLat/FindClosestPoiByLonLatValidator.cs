using FluentValidation;

using gis.Domain.ResultPattern.Errors;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public class FindClosestPoiByLonLatValidator:AbstractValidator<FindClosestPoiByLonLatQuery>
{

    public FindClosestPoiByLonLatValidator()
    {
        RuleFor(x=> x.lat).NotNull()
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Desc);
        RuleFor(x=> x.lon).NotNull()
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Desc);
    }
}