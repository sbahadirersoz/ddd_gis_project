using FluentValidation;

using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Polygon.Queries.FindPolygonById;

public class FindPolygonByIdValidator : AbstractValidator<FindPolygonByIdQuery>
{
    public FindPolygonByIdValidator()
    {
        RuleFor(x => x.id).SetValidator(new IDValidator());
    }
}
