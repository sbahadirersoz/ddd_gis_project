using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Features.Line.Commands.Create;

public class CreateLineCommandValidator:AbstractValidator<CreateLineCommand>
{
    public CreateLineCommandValidator()
    {
        RuleFor(x => x.lineName).SetValidator(new NameValidator());

        RuleFor(x => x.lineDescription)
            .MaximumLength(HardCodedPropertities.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.lineDescription));

        RuleFor(x => x.coordinates).SetValidator(new LineCoordinateBusinessRulesValidator());
        
    }
}