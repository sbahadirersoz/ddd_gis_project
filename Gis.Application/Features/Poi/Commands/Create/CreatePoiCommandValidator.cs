using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Poi.Commands.Create;

public class CreatePoiCommandValidator : AbstractValidator<CreatePoiCommand>
{
    public CreatePoiCommandValidator()
    {
        RuleFor(x => x.PoiName).SetValidator(new NameValidator());
        RuleFor(x=>x.CoordinateDto).SetValidator(new CoordinateValidator());
    }
} 