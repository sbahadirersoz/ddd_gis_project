using System.Data;
using FluentValidation;
using gis.ApplicationLayer.Features.Poi.Commands.Create;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public class CreatePoiCommandValidator:AbstractValidator<CreatePoiCommand>
{
    public CreatePoiCommandValidator()
    {
    }
}