using FluentValidation;
using gis.ApplicationLayer.Common.Validators;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public class FindInRangeLinesByIdValidator:AbstractValidator<FindInRangeLinesByIdQuery>
{
    public FindInRangeLinesByIdValidator()
    {
        RuleFor(x => x.distance).SetValidator(new RangeValidator());
        RuleFor(x => x.id).SetValidator(new IDValidator());
    }
}