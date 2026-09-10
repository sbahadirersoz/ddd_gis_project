using FluentValidation;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Common.Validators.QueryValidators;

public class RangeValidator:AbstractValidator<double>
{
    public RangeValidator()
    {
        RuleFor(x => x).InclusiveBetween(HardCodedPropertities.DistanceUnitRestraints.MinRangeInMeters,
                HardCodedPropertities.DistanceUnitRestraints.MaxRangeInMeters)
            .WithErrorCode(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_DISTANCE_RANGE.Code)
            .WithMessage(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_DISTANCE_RANGE.Desc);
    }
}