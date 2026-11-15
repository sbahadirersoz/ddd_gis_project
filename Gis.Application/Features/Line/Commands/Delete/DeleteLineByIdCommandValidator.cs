using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Commands.Delete;

public class DeleteLineByIdCommandValidator:AbstractValidator<DeleteLineByIdCommand>
{
    public DeleteLineByIdCommandValidator()
    {
        RuleFor(x => x.id).SetValidator(new IDValidator());
    }
}