using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.BusinessRules.PointRules;

public interface IPointRule
{
    Result Execute(POIAggregate aggregate);
}