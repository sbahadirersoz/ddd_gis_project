using gis.Domain.Common;
using gis.Domain.Repositories;
using gis.InfrastructureLayer.Mapper;
using Microsoft.EntityFrameworkCore;

namespace gis.InfrastructureLayer.Repositories;

public abstract class GenericRepository<TDomain, TId, TDbEntity,TMapper>(DbContext context) :
    IRepository<TDomain, TId>
    where TDbEntity : class
    where TDomain : AggregateRoot<TId>
    where TId : EntityId
    where TMapper:IMapperContract<TDomain,TDbEntity>
{
    protected readonly DbSet<TDbEntity> Table = context.Set<TDbEntity>();
    
    protected TDomain ToDomain(TDbEntity entity) => TMapper.ToDomain(entity);
    protected TDbEntity ToDbEntity(TDomain domain) => TMapper.ToDbEntity(domain);
    public async Task AddAsync(TDomain entity, CancellationToken cancellationToken = default)
    {
        
        var dbEntity = ToDbEntity(entity);
        await Table.AddAsync(dbEntity);

    }

    public void Add(TDomain entity)
    {
        var dbEntity = ToDbEntity(entity);
         Table.Add(dbEntity);
    }

    public void Update(TDomain entity)
    {
        var dbEntity = ToDbEntity(entity);
        Table.Update(dbEntity);
    }

    public void Delete(TDomain entity)
    {
        var dbEntity = ToDbEntity(entity);
        Table.Remove(dbEntity);
    }

    public async Task DeleteByIdAsync(TId entityId, CancellationToken cancellationToken = default)
    {
        var entity = await Table.FindAsync(entityId.Value,cancellationToken);
        if (entity != null)
            Table.Remove(entity);
    }

    public async Task<List<TDomain>> GetAllEntitiesAsync(bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
    
        var dbEntities = await query.ToListAsync(cancellationToken);
    
        return dbEntities.Select(ToDomain).ToList();
    }
}