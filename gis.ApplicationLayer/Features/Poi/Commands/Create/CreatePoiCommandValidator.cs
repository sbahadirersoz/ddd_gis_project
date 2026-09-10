using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Poi.Commands.Create;

public class CreatePoiCommandValidator : AbstractValidator<CreatePoiCommand>
{
    public CreatePoiCommandValidator()
    {
        RuleFor(x => x.PoiName).SetValidator(new NameValidator());
        RuleFor(x=>x.CoordinateDto).SetValidator(new CoordinateValidator());
    }
} 