using FluentValidation;

namespace Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;

public class AddHoleToPolygonCommandValidator:AbstractValidator<AddHoleToPolygonCommand>
{
    public AddHoleToPolygonCommandValidator()
    {
        RuleFor(x => x.hole).NotEmpty();
    }
    
}