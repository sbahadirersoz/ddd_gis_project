using System.Data;
using FluentValidation;
using gis.ApplicationLayer.Common.Validators;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public class FindClosestLineByIdValidator:AbstractValidator<FindClosestLineByIdQuery>
{
    public FindClosestLineByIdValidator()
    {
        RuleFor(x => x.id
        ).SetValidator(new IDValidator());
        
    }
}