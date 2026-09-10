using FluentValidation;
using gis.ApplicationLayer.Common.Validators.QueryValidators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public class FindMostClosesCountByIdValidator:AbstractValidator<int>
{
    public FindMostClosesCountByIdValidator()
    {
        RuleFor(x => x).SetValidator(new CountValidator());
    }    
}