using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public class FindClosestByLonLatValidator:AbstractValidator<FindClosestLineByLonLatQuery> 
{
    public FindClosestByLonLatValidator()
    {
        RuleFor(x => x.LonLat).SetValidator(new CoordinateValidator());
    }
}