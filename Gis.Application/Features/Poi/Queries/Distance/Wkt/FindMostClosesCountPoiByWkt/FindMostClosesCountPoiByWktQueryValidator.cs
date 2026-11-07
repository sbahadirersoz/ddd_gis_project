using FluentValidation;

using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

public class FindMostClosesCountPoiByWktQueryValidator:AbstractValidator<FindMostClosesCountPoiByWktQuery>
{
    public FindMostClosesCountPoiByWktQueryValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
       
    }
}