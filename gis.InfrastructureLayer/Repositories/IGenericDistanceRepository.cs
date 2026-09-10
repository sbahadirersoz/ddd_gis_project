using gis.Domain.Common;
using gis.Domain.Contracts;
using gis.InfrastructureLayer.Mapper;
using Microsoft.EntityFrameworkCore;

namespace gis.InfrastructureLayer.Repositories;



/// <summary>
/// Feature İçin fine ama başında düşünülmediği için  implement etmesi daha zor  o yüzden  ileride patchlersem bu
/// </summary>
/// <param name="context"></param>
/// <typeparam name="TDomain"></typeparam>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TDbEntity"></typeparam>
/// <typeparam name="TMapper"></typeparam>
public abstract class IGenericDistanceRepository<TDomain,TId,TDbEntity,TMapper>(DbContext context):IDistanceCalculatorContract<TDomain,TId>

where TDbEntity:class
where TDomain : AggregateRoot<TId>
where TId : EntityId
where TMapper: IMapperContract<TDomain, TDbEntity>
{
    protected readonly DbSet<TDbEntity> Table = context.Set<TDbEntity>();
    
    protected TDomain ToDomain(TDbEntity entity) => TMapper.ToDomain(entity);
    protected TDbEntity ToDbEntity(TDomain domain) => TMapper.ToDbEntity(domain);
    public Task<List<(TDomain entity, double distance)>> FindNearbySameEntityByGivenIdAsync(TId entityId, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<(TDomain entity, double distance)>> FindMostClosesCountByIdAsync(TId entityId, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(TDomain entity, double distance)?> FindClosestEntityByGivenIdAsync(TId entityId, bool tracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}