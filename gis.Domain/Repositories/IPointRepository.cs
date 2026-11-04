using System.Linq.Expressions;
using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;

namespace gis.Domain.Repositories;

public interface IPointRepository:IRepository<POIAggregate,PointID>,IPoiDistanceCalculatorContract

{
    Task<POIAggregate?>FindByWktAsync(WellKnownText wkt,   bool tracking = true, CancellationToken cancellationToken = default);

    Task<bool> IsLonLatCoordinatesExistsAsync(Latitude lat, Longitude lon, CancellationToken cancellationToken = default);
    Task<bool> IsPointNameExistsAsync(PointName pointName, CancellationToken cancellationToken = default);
    
    Task<POIAggregate?>FindPointByIdAsync(PointID entityId,bool tracking = true,CancellationToken cancellationToken = default);
    Task<POIAggregate?>FindPointByPointNameAsync(PointName pointName,bool tracking = true,CancellationToken cancellationToken = default);
    Task UpdateAsync(POIAggregate aggregate, CancellationToken cancellationToken = default);

}