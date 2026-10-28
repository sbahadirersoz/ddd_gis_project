using FluentValidation;
using Gis.Application.Common.Validators;
using Gis.Application.Common.Validators.QueryValidators;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public class FindMostClosesCountByNameValidator:AbstractValidator<FindMostClosesCountByNameQuery>
{
    public FindMostClosesCountByNameValidator()
    {
        RuleFor(x => x.count).SetValidator(new CountValidator());
        RuleFor(x => x.Name).SetValidator(new NameValidator());
    }
}