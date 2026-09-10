using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Commands.Delete;

public class DeleteLineByIdCommandValidator:AbstractValidator<DeleteLineByIdCommand>
{
    public DeleteLineByIdCommandValidator()
    {
        RuleFor(x => x.id.Value).SetValidator(new IDValidator());
    }
}