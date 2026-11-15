using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;

namespace gis.Domain.Contracts;

public interface ILineDistanceCalculatorContract:IDistanceCalculatorContract<LineAggregate,LineID>
{
    
    
    
    
    Task<List<LineAggregate>> FindLineStringsIncludingThisLonLat(Longitude lon,Latitude lat,
         bool tracking = true, CancellationToken cancellationToken = default);
 Task<List<(LineAggregate agg ,double distance )>> FindMostClosesCountByLonLatAsync(Longitude lon,Latitude lat,int count,
         bool tracking = true, CancellationToken cancellationToken = default);
 Task<List<(LineAggregate agg ,double distance )>> FindInRangeLinesByLonLatAsync(Longitude lon,Latitude lat,
        double distance, bool tracking = true, CancellationToken cancellationToken = default);
Task<(LineAggregate agg ,double distance )?> FindClosestByLonLatAsync(Longitude lon,Latitude lat, bool tracking = true, CancellationToken cancellationToken = default);

}