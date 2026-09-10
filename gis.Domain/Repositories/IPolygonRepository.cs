using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Contracts.Distance;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;

namespace gis.Domain.Repositories;

public interface IPolygonRepository : IRepository<PolygonAggregate, PolygonID>,IDistanceCalculatorContract<PolygonAggregate,PolygonID>
{
    Task<PolygonAggregate?> FindByIdAsync(PolygonID id, bool tracking = true, CancellationToken cancellationToken = default);
    Task<bool> IsPolygonNameExistsAsync(PolygonName polygonName, CancellationToken cancellationToken = default);
    Task UpdateAsync(PolygonAggregate entity, CancellationToken cancellationToken = default);
    Task<PolygonAggregate> FindByNameAsync(PolygonName name, bool tracking = true, CancellationToken cancellationToken = default);
}
