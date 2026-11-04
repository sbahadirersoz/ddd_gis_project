using System.Linq.Expressions;
using gis.Domain.Common;

namespace gis.Domain.Repositories;

public interface IRepository<TEntity,TId>
where TEntity:AggregateRoot<TId>
where TId:EntityId
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task DeleteByIdAsync(TId entityId, CancellationToken cancellationToken = default);
    Task<List<TEntity>>GetAllEntitiesAsync(bool tracking = true , CancellationToken cancellationToken = default);
    
}