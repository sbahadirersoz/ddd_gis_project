using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;

namespace gis.Domain.Repositories;

public interface IPoiDistanceCalculatorContract : IDistanceCalculatorContract<POIAggregate, PointID>
{
    
    Task<List<(POIAggregate agg, double distance)>> FindInRangePoisByIdAsync(PointID id, double distance,
        bool tracking = true, CancellationToken cancellationToken = default);
    
    

    Task<(POIAggregate agg, double distance)?> FindClosestPoiByName(PointName name, bool tracking = true,
        CancellationToken cancellationToken = default);
    Task<List<(POIAggregate agg, double distance)>> FindInRangePoisByNameAsync(PointName pointName, double distance,
        bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(POIAggregate agg, double distance)>> FindMostClosesCountByPoiName(PointName name, int count,
        bool tracking = true, CancellationToken cancellationToken = default);



    Task<(POIAggregate agg, double distance)> FindClosestPoiByWktAsync(WellKnownText wellKnownText,
        bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(POIAggregate agg, double distance)>> FindInRangePoisByWktAsync(WellKnownText wellKnownText,
        double distance, bool tracking = true, CancellationToken cancellationToken = default);
Task<List<(POIAggregate agg, double distance)>> FindMostClosesCountByWktAsync(WellKnownText wellKnownText,
        int count, bool tracking = true, CancellationToken cancellationToken = default);

    Task<(POIAggregate agg, double distance)> FindClosestPoiByGivenLonLatAsync(Longitude lon  , Latitude lat, bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<(POIAggregate agg, double distance)>> FindInRangePoisByLonLatAsync(Longitude lon  , Latitude lat,
        double distance, bool tracking = true, CancellationToken cancellationToken = default);
Task<List<(POIAggregate agg, double distance)>> FindMostClosesCountByLonLatAsync(Longitude lon  , Latitude lat,
        int count, bool tracking = true, CancellationToken cancellationToken = default);





}
