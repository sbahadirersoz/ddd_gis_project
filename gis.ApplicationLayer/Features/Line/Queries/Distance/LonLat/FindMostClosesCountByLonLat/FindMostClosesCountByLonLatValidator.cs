using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public class FindMostClosesCountByLonLatValidator:AbstractValidator<FindMostClosesLineCountByLonLatQuery>

{
    public FindMostClosesCountByLonLatValidator()
    {
        RuleFor(x=> x.count).SetValidator(new  CountValidator());
        RuleFor(x=> x.lonLat).SetValidator(new  CoordinateValidator());
    }    
}