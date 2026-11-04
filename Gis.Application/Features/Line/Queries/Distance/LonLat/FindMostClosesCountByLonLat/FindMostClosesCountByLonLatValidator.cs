using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public class FindMostClosesCountByLonLatValidator:AbstractValidator<FindMostClosesLineCountByLonLatQuery>

{
    public FindMostClosesCountByLonLatValidator()
    {
        RuleFor(x=> x.count).SetValidator(new  CountValidator());
        RuleFor(x=> x.lonLat).SetValidator(new  CoordinateValidator());
    }    
}