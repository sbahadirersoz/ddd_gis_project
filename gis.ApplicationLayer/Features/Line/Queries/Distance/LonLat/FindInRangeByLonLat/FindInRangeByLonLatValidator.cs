using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public class FindInRangeByLonLatValidator:AbstractValidator<FindInRangeLineByLonLatQuery> 
{
    public FindInRangeByLonLatValidator()
    {
        RuleFor(x => x.LonLat).SetValidator(new CoordinateValidator());
        RuleFor(x => x.DistanceInMeter).SetValidator(new RangeValidator());
        
    }
}