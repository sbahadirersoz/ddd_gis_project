using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public class FindInRangeLinesByIdValidator:AbstractValidator<FindInRangeLinesByIdQuery>
{
    public FindInRangeLinesByIdValidator()
    {
        RuleFor(x => x.DistanceByMeter).SetValidator(new RangeValidator());
        RuleFor(x => x.id).SetValidator(new IDValidator());
    }
}