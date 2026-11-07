using FluentValidation;

using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

using gis.Domain.ResultPattern.Errors;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

public class FindInRangePoisByLonLatValidator:AbstractValidator<FindInRangePoisByLonLatQuery>
{
    public FindInRangePoisByLonLatValidator()
    {
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
        RuleFor(x => x.lon).NotNull()
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Desc);
        RuleFor(x => x.lat).NotNull()
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Desc);
    }
}