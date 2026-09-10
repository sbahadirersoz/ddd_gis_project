using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Commands.Update;

public class UpdateLineCommandValidator:AbstractValidator<UpdateLineCommand>
{
    public UpdateLineCommandValidator()
    {
        RuleFor(x => x.Coordinates).SetValidator(new LineCoordinateBusinessRulesValidator()).When(x=> x.Coordinates != null);
        RuleFor(x => x.id).SetValidator(new IDValidator());
        RuleFor(x => x.LineName).SetValidator(new NameValidator()).When(x=> x.LineName != null);
        RuleFor(x => (int)x.Status.Value).SetValidator(new StatusValidator()).When(x=>x.Status != null);
    }
}