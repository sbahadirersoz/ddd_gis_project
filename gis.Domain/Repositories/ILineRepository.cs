using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;

namespace gis.Domain.Repositories;

public interface ILineRepository:IRepository<LineAggregate,LineID>
{
    
}