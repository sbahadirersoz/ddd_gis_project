using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;

namespace gis.Domain.Contracts.Distance;

public interface IPolygonDistanceCalculatorContract:IDistanceCalculatorContract<PolygonAggregate, PolygonID>
{
    
}