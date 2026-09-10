using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;

namespace gis.Domain.Contracts;

public interface ILineDistanceCalculatorContract:IDistanceCalculatorContract<LineAggregate,LineID>
{
    
    Task<(LineAggregate agg, double distance)> FindClosestLineByWktAsync(WellKnownText wellKnownText,
        bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(LineAggregate agg, double distance)>> FindInRangeLinesByWktAsync(WellKnownText wellKnownText,
        double distance, bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(LineAggregate agg, double distance)>> FindMostClosesCountByWktAsync(WellKnownText wellKnownText,
        int count, bool tracking = true, CancellationToken cancellationToken = default);
    
    Task<(LineAggregate agg, double distance)> FindClosestLineByNameAsync(LineName name,
        bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(LineAggregate agg, double distance)>> FindInRangeLinesByLineNameAsync(LineName name,
        double distance, bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(LineAggregate agg, double distance)>> FindMostClosesCountByLineNameAsync(LineName name,
        int count, bool tracking = true, CancellationToken cancellationToken = default);
    
    
    
    Task<List<LineAggregate>> FindLineStringsIncludingThisLonLat(Longitude lon,Latitude lat,
        int count, bool tracking = true, CancellationToken cancellationToken = default);
 Task<List<(LineAggregate agg ,double distance )>> FindMostClosesCountByLonLatAsync(Longitude lon,Latitude lat,
        int count, bool tracking = true, CancellationToken cancellationToken = default);
 Task<List<(LineAggregate agg ,double distance )>> FindInRangeLinesByLonLatAsync(Longitude lon,Latitude lat,
        double distance, bool tracking = true, CancellationToken cancellationToken = default);
Task<(LineAggregate agg ,double distance )> FindClosestByLonLatAsync(Longitude lon,Latitude lat, bool tracking = true, CancellationToken cancellationToken = default);

}