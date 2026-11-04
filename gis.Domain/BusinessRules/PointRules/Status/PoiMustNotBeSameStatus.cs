using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PointRules;

public class PoiMustNotBeSameStatus:IPointRule
{

    private readonly POIStatus status;
    public PoiMustNotBeSameStatus(POIStatus status)
    {
        this.status = status;
    }

    public Result Execute(POIAggregate aggregate)
         => (aggregate.Status == status)? Result.Failure(BusinessRuleErrors.ENTITY_STATUS_ALREADY_GIVEN_PARAMETER) : Result.Success(); 
    }
