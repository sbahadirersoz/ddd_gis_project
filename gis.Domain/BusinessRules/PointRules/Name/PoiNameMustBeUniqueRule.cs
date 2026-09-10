using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PointRules.Name;

public class PoiNameMustBeUniqueRule:IPointRule
{
    
    private readonly PointName _poiName;

    public PoiNameMustBeUniqueRule(PointName poiName)
    {
        _poiName = poiName;
    }


    public Result Execute(POIAggregate aggregate)
        => aggregate.PointName.Value == _poiName.Value
            ? Result.Failure(DomainErrors.POIErrors.PointName.SAME_VALUE_PROVIDED)
            : Result.Success();
}