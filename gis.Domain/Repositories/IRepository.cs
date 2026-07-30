using gis.Domain.Common;

namespace gis.Domain.Repositories;

public interface IRepository<TEntity>
where TEntity:AggregateRoot<EntityId>
{
    
}