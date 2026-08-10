using System.Linq.Expressions;
using gis.Domain.Common;

namespace gis.Domain.Repositories;

public interface IRepository<TEntity,TId>
where TEntity:AggregateRoot<TId>
where TId:EntityId
{
    Task<TEntity?> FindByEntityExpression(Expression<Func<TEntity, bool>> expression , bool tracking =true);
    Task<List<TEntity>?> FindEntitiesByExpression(Expression<Func<TEntity, bool>> expression , bool tracking =true);
    Task<TEntity?>FindEntityByIdAsync(EntityId entityId);
    Task<bool> UpdateEntityAsync(TEntity entity,CancellationToken cancellationToken = default,bool tracking =true);
}