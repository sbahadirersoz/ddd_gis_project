using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PointRules;

public class PoiMustNotBeDeletedRule:IPointRule
{
    public PoiMustNotBeDeletedRule()
    {
    }

    public Result Execute(POIAggregate aggregate)
        => aggregate.Status == POIStatus.SOFT_DELETED
            ? Result.Failure(BusinessRuleErrors.ENTITY_ALREADY_SOFT_DELETED)
            : Result.Success();
}