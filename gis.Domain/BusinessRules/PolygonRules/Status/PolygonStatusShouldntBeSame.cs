using gis.Domain.Aggregates;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PolygonRules.Status;

public class PolygonStatusShouldntBeSame:IPolygonRule
{
    private readonly PolygonAggregate agg;
    private readonly PolygonStatus status;

    public PolygonStatusShouldntBeSame(PolygonStatus status, PolygonAggregate agg)
    {
        this.status = status;
        this.agg = agg;
    }

    public Result Execute() => agg.Status != status
        ? Result.Success()
        : Result.Failure(BusinessRuleErrors.ENTITY_STATUS_ALREADY_GIVEN_PARAMETER);
}