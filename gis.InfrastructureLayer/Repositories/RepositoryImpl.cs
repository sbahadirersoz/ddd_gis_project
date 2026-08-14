using System.Linq.Expressions;
using gis.Domain.Common;
using gis.Domain.Repositories;
using gis.InfrastructureLayer.DB.Context;
using Microsoft.EntityFrameworkCore;

namespace gis.InfrastructureLayer.Repositories;

public class RepositoryImpl<TEntity,TId>:IRepository<TEntity,TId> 
where TEntity:AggregateRoot<TId>
where TId:EntityId
{
    private readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> Table;

    public RepositoryImpl( AppDbContext dbContext)
    {
        _dbContext = dbContext;
        Table = _dbContext.Set<TEntity>();
    }


    public async Task<TEntity?> FindByEntityExpressionAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable(): Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<List<TEntity>> FindEntitiesByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable(): Table.AsNoTracking();
        return await query.Where(expression).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> FindEntityByIdAsync(TId entityId, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable(): Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.Id.Equals(entityId), cancellationToken);
    }

    public async Task<List<TEntity>> GetAllEntitiesAsync(bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        return await query.ToListAsync(cancellationToken);
    }

     public async  Task  AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        Table.Add(entity);
    }

    public void Update(TEntity entity)
    {
        Table.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Table.Remove(entity);
    }

    public  async Task DeleteByIdAsync(TId entityId, CancellationToken cancellationToken = default)
    {
        var result = await Table.FirstOrDefaultAsync(x=> x.Id.Equals(entityId),cancellationToken);
        if (result != null)
            Table.Remove(result);
    }

}