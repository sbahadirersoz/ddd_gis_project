using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.InfrastructureLayer.DB.Context;

namespace gis.InfrastructureLayer.Repositories;

public class PointRepository:RepositoryImpl<POIAggregate,PointID>,IPointRepository
{
    public PointRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> IsLonLatCoordinatesExistsAsync(Latitude lat, Longitude lon, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsPointNameExistsAsync(PointName pointName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsPointExists(PointName pointName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}