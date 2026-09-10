using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Poi.Commands.Delete;

public class DeletePoiCommandValidator:AbstractValidator<DeletePoiCommand>
{
    public DeletePoiCommandValidator()
    {
        RuleFor(x => x.id.Value).SetValidator(new IDValidator());
    }
}