using gis.Domain.Aggregates;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PolygonRules.Status;

public class PolygonMustBeNotSoftDeleted(PolygonAggregate agg):IPolygonRule
{

    private readonly PolygonAggregate agg = agg;

    public Result Execute()
        => (agg.Status == PolygonStatus.SOFT_DELETED)
            ? Result.Failure(BusinessRuleErrors.ENTITY_SOFT_DELETED)
            : Result.Success();
}