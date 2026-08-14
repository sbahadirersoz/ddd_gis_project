using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;

namespace gis.Domain.Repositories;

public interface IPointRepository:IRepository<POIAggregate,PointID>
{

    Task<bool> IsLonLatCoordinatesExistsAsync(Latitude lat, Longitude lon, CancellationToken cancellationToken = default);
    Task<bool> IsPointNameExistsAsync(PointName pointName, CancellationToken cancellationToken = default);
    Task<bool> IsPointExists(PointName pointName, CancellationToken cancellationToken = default);
}