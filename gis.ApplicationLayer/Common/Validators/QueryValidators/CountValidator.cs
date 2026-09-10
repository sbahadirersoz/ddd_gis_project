using FluentValidation;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Common.Validators.QueryValidators;

public class CountValidator:AbstractValidator<int>
{
    public CountValidator()
    {
        RuleFor(x => x).NotEmpty()
            .InclusiveBetween(HardCodedPropertities.DistanceUnitRestraints.MinCountForMostClosesQuery,
                HardCodedPropertities.DistanceUnitRestraints.MaxCountForMostClosesQuery)
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_QUERY_COUNT.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_QUERY_COUNT.Desc);
    }
}