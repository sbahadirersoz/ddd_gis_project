using System.Data;

using FluentValidation;

using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

public class FindMostClosesCountPoiByLonLatValidator:AbstractValidator<FindMostClosesCountPoiByLonLatQuery>
{
    public FindMostClosesCountPoiByLonLatValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
        RuleFor(x => x.lonlat).SetValidator(new CoordinateValidator());
    }
}