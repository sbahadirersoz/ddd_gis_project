using FluentValidation;
using gis.Domain.Contracts;

namespace Gis.Application.Features.Polygon.Commands;

public class CreatePolygonCommandValidator:AbstractValidator<CreatePolygonCommand>
{

    public CreatePolygonCommandValidator(ITopologySuitePolygonValidationContract contract)
    {
    }
}