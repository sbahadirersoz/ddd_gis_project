using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Poi.Commands.Update;

public class UpdatePoiCommandValidator: AbstractValidator<UpdatePoiCommand>

{
    public UpdatePoiCommandValidator()
    {

        RuleFor(x => x.id).SetValidator(new IDValidator());

        RuleFor(x => x.PoiName).SetValidator(new NameValidator())
            .When(x => x.PoiName != null);
        RuleFor(x=>x.CoordinateDto).SetValidator(new CoordinateValidator())
            .When(x=>x.CoordinateDto != null);
        RuleFor(x=>(int)x.Status).SetValidator(new StatusValidator())
            .When(x=>x.Status != null);
    }
}