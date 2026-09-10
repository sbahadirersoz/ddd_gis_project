using FluentValidation;
using gis.ApplicationLayer.Dtos;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Common.Validators;

public class LineCoordinateBusinessRulesValidator:AbstractValidator<List<CoordinateDto>>
{
    public LineCoordinateBusinessRulesValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .Must(x => x != null && x.Count > 1)
            .WithErrorCode(BusinessRuleErrors.LineRules.INSUFFICENT_POINTS.Code)
            .WithMessage(BusinessRuleErrors.LineRules.INSUFFICENT_POINTS.Desc);

        RuleForEach(x => x)
            .ChildRules
            (point =>{
                    point.RuleFor(x => x.Longitude).InclusiveBetween
                        (
                            HardCodedPropertities.CoordinatePropertities.MinLongitude,
                            HardCodedPropertities.CoordinatePropertities.MaxLongitude
                        )
                        .WithErrorCode(DomainErrors.LONGITUDE_COORDINATE_IS_NOT_IN_RANGE_ERROR.Code)
                        .WithMessage(DomainErrors.LONGITUDE_COORDINATE_IS_NOT_IN_RANGE_ERROR.Desc);

                    point.RuleFor(x => x.Latitude).InclusiveBetween
                        (
                            HardCodedPropertities.CoordinatePropertities.MinLatitude,
                            HardCodedPropertities.CoordinatePropertities.MaxLatitude
                        )
                        .WithErrorCode(DomainErrors.LATITUDE_COORDINATE_IS_NOT_IN_RANGE_ERROR.Code)
                        .WithMessage(DomainErrors.LATITUDE_COORDINATE_IS_NOT_IN_RANGE_ERROR.Desc);
                }
            );
    }
    
}