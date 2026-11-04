using gis.Domain.Aggregates;
using gis.Domain.BusinessRules.LineRules;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PointRules;

public class LineMustNotBeDeletedRule:ILineRule
{

    private readonly LineAggregate _agg;
    public LineMustNotBeDeletedRule(LineAggregate agg)
    {
        _agg = agg;
    }

    public Result Execute()
        => _agg.LineStatus == LineStatus.SOFT_DELETED
            ? Result.Failure(BusinessRuleErrors.ENTITY_ALREADY_SOFT_DELETED)
            : Result.Success();
}