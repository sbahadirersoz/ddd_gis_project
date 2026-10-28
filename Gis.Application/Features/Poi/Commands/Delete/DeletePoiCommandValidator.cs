using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Poi.Commands.Delete;

public class DeletePoiCommandValidator:AbstractValidator<DeletePoiCommand>
{
    public DeletePoiCommandValidator()
    {
        RuleFor(x => x.id.Value).SetValidator(new IDValidator());
    }
}