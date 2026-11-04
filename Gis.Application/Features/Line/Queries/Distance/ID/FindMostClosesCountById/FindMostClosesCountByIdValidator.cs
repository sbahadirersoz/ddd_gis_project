using FluentValidation;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public class FindMostClosesCountByIdValidator:AbstractValidator<int>
{
    public FindMostClosesCountByIdValidator()
    {
        RuleFor(x => x).SetValidator(new CountValidator());
    }    
}