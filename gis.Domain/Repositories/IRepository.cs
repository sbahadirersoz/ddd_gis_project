using System.Linq.Expressions;
using gis.Domain.Common;

namespace gis.Domain.Repositories;

public interface IRepository<TEntity,TId>
where TEntity:AggregateRoot<TId>
where TId:EntityId
{
    Task<TEntity?> FindByEntityAsyncExpression(Expression<Func<TEntity, bool>> expression , bool tracking =true  , CancellationToken cancellationToken = default );
    Task<List<TEntity>> FindEntitiesByExpression(Expression<Func<TEntity, bool>> expression, bool tracking = true,
        CancellationToken cancellationToken = default);
    Task<TEntity?>FindEntityByIdAsync(EntityId entityId,bool tracking = true,CancellationToken cancellationToken = default);
    Task<List<TEntity>>GetAllEntities();
    Task<bool> UpdateEntityAsync(TEntity entity,CancellationToken cancellationToken = default,bool tracking =true);
    Task<bool>DeleteEntityByIdAsync(TId entityId,CancellationToken cancellationToken = default,bool tracking =true);
    Task<bool>DeleteEntityById(TId entityId,CancellationToken cancellationToken = default,bool tracking =true);
    Task<bool>DeleteEntity(TEntity entityId,CancellationToken cancellationToken = default,bool tracking =true);
}