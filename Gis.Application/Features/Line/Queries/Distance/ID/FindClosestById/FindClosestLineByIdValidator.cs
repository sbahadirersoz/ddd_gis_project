using System.Data;
using FluentValidation;
using Gis.Application.Common.Validators;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public class FindClosestLineByIdValidator:AbstractValidator<FindClosestLineByIdQuery>
{
    public FindClosestLineByIdValidator()
    {
        RuleFor(x => x.id
        ).SetValidator(new IDValidator());
        
    }
}