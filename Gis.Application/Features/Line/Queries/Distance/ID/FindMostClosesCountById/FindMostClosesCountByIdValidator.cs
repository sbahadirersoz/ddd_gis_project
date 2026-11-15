using FluentValidation;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public class FindMostClosesCountByIdValidator:AbstractValidator<FindMostClosesCountByIdQuery>
{
    public FindMostClosesCountByIdValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
    }    
}