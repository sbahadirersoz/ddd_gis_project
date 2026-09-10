using FluentValidation;
using gis.Domain.Contracts;

namespace gis.ApplicationLayer.Features.Polygon.Commands;

public class CreatePolygonCommandValidator:AbstractValidator<CreatePolygonCommand>
{

    public CreatePolygonCommandValidator(ITopologySuitePolygonValidationContract contract)
    {
    }
}