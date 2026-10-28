using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public class FindInRangeByLonLatValidator:AbstractValidator<FindInRangeLineByLonLatQuery> 
{
    public FindInRangeByLonLatValidator()
    {
        RuleFor(x => x.LonLat).SetValidator(new CoordinateValidator());
        RuleFor(x => x.DistanceInMeter).SetValidator(new RangeValidator());
        
    }
}