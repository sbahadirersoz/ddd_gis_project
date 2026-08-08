using FluentValidation;

namespace gis.ApplicationLayer.Features.Poi.Commands.Create;

public class CreatePoiCommandValidator : AbstractValidator<CreatePoiCommand>
{
    public CreatePoiCommandValidator()
    {
    }
}