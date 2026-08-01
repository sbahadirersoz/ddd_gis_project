using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;

namespace gis.Domain.Repositories;

public interface IPointRepository
{

    Task<bool> IsLatLonCoordinatesExistsAsync(Latitude lat, Longitude lon, CancellationToken cancellationToken = default);
    Task<bool> IsPointNameExistsAsync(PointName pointName, CancellationToken cancellationToken = default);

    void SaveAsync(POIAggregate poiAggregate);
}