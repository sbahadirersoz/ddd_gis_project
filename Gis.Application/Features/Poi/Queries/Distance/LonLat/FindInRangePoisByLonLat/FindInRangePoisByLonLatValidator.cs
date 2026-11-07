using FluentValidation;

using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

public class FindInRangePoisByLonLatValidator:AbstractValidator<FindInRangePoisByLonLatQuery>
{
    public FindInRangePoisByLonLatValidator()
    {
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
    }
}