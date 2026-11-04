using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public class FindClosestByLonLatValidator:AbstractValidator<FindClosestLineByLonLatQuery> 
{
    public FindClosestByLonLatValidator()
    {
        RuleFor(x => x.LonLat).SetValidator(new CoordinateValidator());
    }
}