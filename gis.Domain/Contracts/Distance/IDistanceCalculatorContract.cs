
namespace gis.Domain.Contracts;

public interface IDistanceCalculatorContract<TEntity,TId>
{
    Task<List<(TEntity  entity,double distance)>> FindNearbySameEntityByGivenIdAsync(TId entityId, double distance, bool tracking = true,
        CancellationToken cancellationToken = default);
    
    Task<List<(TEntity  entity,double distance)>> FindMostClosesCountByIdAsync(TId entityId,int count, bool tracking = true,
        CancellationToken cancellationToken = default);
    Task<(TEntity entity ,double distance)?>FindClosestEntityByGivenIdAsync(TId entityId,bool tracking = true,CancellationToken cancellationToken = default);

    
    
}