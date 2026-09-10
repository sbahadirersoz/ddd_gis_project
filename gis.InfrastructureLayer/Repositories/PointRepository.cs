using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.InfrastructureLayer.DB.Context;
using gis.InfrastructureLayer.Entity;
using gis.InfrastructureLayer.HelperMethods;
using gis.InfrastructureLayer.Mapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace gis.InfrastructureLayer.Repositories;

public class PointRepository : GenericRepository<POIAggregate, PointID, PoiEntity, PoiMapper>, IPointRepository
{
    public PointRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    public WKTReader reader = new();



    #region Exist Queries

    public async Task<bool> IsLonLatCoordinatesExistsAsync(Latitude lat, Longitude lon,
        CancellationToken cancellationToken = default)
    {
        var aggregateCoordinatesToEntityPoint = CoordinateFactory.LonLatToPoint(lon, lat);
        var any = await Table.AnyAsync(x => x.Point.IsWithinDistance(aggregateCoordinatesToEntityPoint, 1.0),
            cancellationToken);
        return any;
    }

    public async Task<bool> IsPointNameExistsAsync(PointName pointName, CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(x => x.Name.Equals(pointName.Value), cancellationToken);
    }

    #endregion


    #region Find By Field 

    public async Task<POIAggregate?> FindPointByIdAsync(PointID entityId, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var firstOrDefault =
            await query.FirstOrDefaultAsync(x => x.Id.Equals(entityId.Value), cancellationToken: cancellationToken);
        return (firstOrDefault is not null) ? ToDomain(firstOrDefault) : null;
    }

    public async Task<POIAggregate?> FindPointByPointNameAsync(PointName pointName, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var firstOrDefault =
            await query.FirstOrDefaultAsync(x => x.Name.Equals(pointName.Value), cancellationToken: cancellationToken);
        return (firstOrDefault is not null) ? ToDomain(firstOrDefault) : null;
    }

    
    public async Task<List<(POIAggregate agg , double  distance)>> FindInRangePoisByIdAsync(PointID id, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity =
            await query.FirstOrDefaultAsync(x => x.Id.Equals(id.Value), cancellationToken: cancellationToken);
        if (entity == null) return [];
        var poiEntities = query
            .Where(x => !x.Id.Equals(id.Value) && x.Point.IsWithinDistance(entity.Point, distance)).Select( x=> 
                new
                {
                    Entity = x,
                    Distance = x.Point.Distance(entity.Point),
                }
            ).OrderBy(x=> x.Distance).ToList();
        return poiEntities.Select(r => (agg :ToDomain(r.Entity),distance: r.Distance)
        ).ToList();
    }

    #endregion


    #region Update For Attachment

    public async Task UpdateAsync(POIAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var entity = await Table.FirstOrDefaultAsync(x => x.Id == aggregate.Id.Value, cancellationToken);
        if (entity is not null)
        {
            entity.ChangeStatus((int)aggregate.Status);
            entity.ChangeCoords(aggregate.CoordinateValueObject.Latitude, aggregate.CoordinateValueObject.Longitude);
            entity.ChangeDesc(aggregate.PointDesc.Value);
        }
    }

    #endregion



    #region Distance By ID

      public async Task<List<(POIAggregate  entity,double distance)>> FindNearbySameEntityByGivenIdAsync(PointID entityId, double distance,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity =
            await query.FirstOrDefaultAsync(x => x.Id.Equals(entityId.Value), cancellationToken: cancellationToken);
        if (entity == null) return default;
        var poiEntities = await query
                .Where(x => !x.Id.Equals(entityId.Value) && x.Point.IsWithinDistance(entity.Point, distance)).Select(x=>new {entity = x, distance = entity.Point.Distance(x.Point)}).OrderBy(x=> x.distance)
                .ToListAsync(cancellationToken: cancellationToken);
        return poiEntities.Select(x => (ToDomain(x.entity), x.distance)).ToList();
    }


    public async Task<List<(POIAggregate entity ,double distance)>> FindMostClosesCountByIdAsync(PointID entityId, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity =
            await query.FirstOrDefaultAsync(x => x.Id.Equals(entityId.Value), cancellationToken: cancellationToken);
        if (entity == null) return [];
        {
            var poiEntities = query
                .Where(x => !x.Id.Equals(entityId.Value))
                .OrderBy(x => x.Point.Distance(entity.Point))
                .Take(count)
                .Select(x => new
                    {
                        agg = x,
                        distance = x.Point.Distance(entity.Point)
                    }
                ).ToList();
            return poiEntities.Select(r => (agg: ToDomain(r.agg), distance: r.distance)).ToList();
        }
    }

    public async Task<(POIAggregate entity, double distance)?> FindClosestEntityByGivenIdAsync(PointID entityId, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.FirstOrDefaultAsync(x => x.Id.Equals(entityId.Value), cancellationToken: cancellationToken);
        if (entity == null) return null;
        var result = 
            await query.Where(x => !x.Id.Equals(entityId.Value))
            .OrderBy(x => x.Point.Distance(entity.Point))
            .Select(x => new
                {
                    Entity = x,
                    Distance = x.Point.Distance(entity.Point)
                }
            ).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (result == null) return null;
        return (ToDomain(result.Entity), result.Distance);




    }



    #endregion


    #region  Distance By Name

    public async Task<List<(POIAggregate agg , double distance)>> FindInRangePoisByNameAsync(PointName pointName, double distance,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity =
            await query.FirstOrDefaultAsync(x => x.Name.Equals(pointName.Value), cancellationToken: cancellationToken);
        if (entity == null) return [];
        {
            var poiEntities = query
                .Where(x => !x.Name.Equals(pointName.Value) && x.Point.IsWithinDistance(entity.Point, distance))
                .OrderBy(x => x.Point.Distance(entity.Point))
                .Select(x => new
                    {
                        Entity = x,
                        Distance = x.Point.Distance(entity.Point)

                    }
                ).ToList();
            return poiEntities.Select(x => (agg: ToDomain(x.Entity), distance: x.Distance)).ToList();
        }

    }

    public async Task<(POIAggregate agg , double  distance)?> FindClosestPoiByName(PointName name, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.FirstOrDefaultAsync(x => x.Name.Equals(name.Value), cancellationToken: cancellationToken);
        if (entity == null) return null;
        var result = await query.Where(x => !x.Id.Equals(entity.Id)).Select(x => new
            {
                entity = x,
                distance = x.Point.Distance(entity.Point)
            }
        ).OrderBy(x => x.distance).FirstOrDefaultAsync(cancellationToken: cancellationToken);    
        
        return (ToDomain(result.entity),result.distance);
    }
    
      public async Task<List<(POIAggregate agg , double  distance)>> FindMostClosesCountByPoiName(PointName name, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity =
            await query.FirstOrDefaultAsync(x => x.Name.Equals(name.Value), cancellationToken: cancellationToken);
        if (entity == null) return null;
        var list = await Table.Where(x => x.Name != name.Value).Select(x=> new
                {
                    entity = x,
                    distance = entity.Point.Distance(x.Point)
                }
        ).OrderBy(x => x.entity.Point.Distance(entity.Point))
            .Take(count).ToListAsync(cancellationToken: cancellationToken);
            
        return list.Select(r=> (entity : ToDomain(r.entity),distance : r.distance)).ToList();
            
    }


    #endregion

    #region WKT

    
    
    /// <summary>
    /// KOD  BURADA PATLIYOR NULL DÖNÜYOR PASO
    /// </summary>
    /// <param name="wellKnownText"></param>
    /// <param name="tracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(POIAggregate agg, double distance)> FindClosestPoiByWktAsync(
        WellKnownText wellKnownText, 
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        var targetPoint = (Point)reader.Read(wellKnownText.Value);

        var nearest = await query
            .Where(x=> !x.Point.Equals(targetPoint))
                
            .Select(x => new 
            { 
                Entity = x, 
                Distance = x.Point.Distance(targetPoint) 
            })
            .Where(x => x.Distance > 0.000001)
            .OrderBy(x => x.Distance)
            .FirstOrDefaultAsync(cancellationToken);

        if (nearest == null) 
            return default;
    
        return (ToDomain(nearest.Entity), nearest.Distance);
    }
    public async Task<POIAggregate?> FindByWktAsync(WellKnownText wkt,bool tracking =true, CancellationToken  cancellationToken = default)
    {
        
        var targetPoint = (Point)reader.Read(wkt.Value);
        var query = tracking ?  Table.AsQueryable() : Table.AsNoTracking();
        var entity =  await query.FirstOrDefaultAsync(x => x.Point == targetPoint, cancellationToken: cancellationToken);
        return (entity == null) ? null : ToDomain(entity);
    }
    


  

    public async Task<List<(POIAggregate agg, double distance)>> FindInRangePoisByWktAsync(WellKnownText wellKnownText, double distance,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var targetPoint = (Point)reader.Read(wellKnownText.Value);
        var entity =
            await query.FirstOrDefaultAsync(x => x.Point.Equals(targetPoint),
                cancellationToken: cancellationToken);
        if (entity == null) return new();
        var poiEntities = query
            .Where(x => !x.Point.Equals(targetPoint) && x.Point.IsWithinDistance(entity.Point, distance))
            .Select(x => new
                {
                    entity = x,
                    distance = entity.Point.Distance(x.Point)

                }
            )
            .OrderBy(x => x.entity.Point.Distance(entity.Point))
            .ToList();
        return poiEntities.Select(x => (ToDomain(x.entity), x.distance)).ToList();

    }

    public async Task<List<(POIAggregate agg, double distance)>> FindMostClosesCountByWktAsync(WellKnownText wellKnownText, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var target = (Point)reader.Read(wellKnownText.Value);
        var entity = await query.FirstOrDefaultAsync(x => x.Point == target, cancellationToken: cancellationToken);
        if (entity == null) return new List<(POIAggregate agg, double distance)>();
        var list = await query.Where(x => x.Id != entity.Id)
            .Select(x => new { entity = x, distance = entity.Point.Distance(x.Point) }).OrderBy(x => x.distance)
            .Take(count).ToListAsync(cancellationToken: cancellationToken);
        return list.Select(x => (agg: ToDomain(x.entity), distance: x.distance)).ToList();
    }



    #endregion
    #region Lon Lat Queries
    public async Task<(POIAggregate agg, double distance)> FindClosestPoiByGivenLonLatAsync(
        Longitude lon, 
        Latitude lat, 
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var givenPoint = new Point(lon.Value, lat.Value) { SRID = 4326 };
        if (givenPoint.IsEmpty) 
            return default;

        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        var result = await query
            .Select(x => new
            {
                Entity = x,
                Distance = x.Point.Distance(givenPoint)
            })
            .Where(x => x.Distance > 0.000001)
            .OrderBy(x => x.Distance)         
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null || result.Entity == null) 
            return default;

        return (ToDomain(result.Entity), result.Distance);
    }

    public async Task<List<(POIAggregate agg, double distance)>> FindInRangePoisByLonLatAsync(Longitude lon, Latitude lat, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var givenPoint = new Point(lon.Value,lat.Value) {SRID = 4326};
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var result =   await query.Where(x =>  givenPoint.IsWithinDistance(x.Point, distance)).Select(x=>new  { entity = x,distance = givenPoint.Distance(x.Point) }).OrderBy(x => x.distance).ToListAsync(cancellationToken);
        if (result == null || result.Count == 0) return default;
        return result.Select(x=> (ToDomain(x.entity), x.distance)).ToList();
        
        
        
        

    }

    public async Task<List<(POIAggregate agg, double distance)>> FindMostClosesCountByLonLatAsync(Longitude lon, Latitude lat, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var givenPoint = new Point(lon.Value,lat.Value) {SRID = 4326};
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var result = await query.Select(x=> new { Entity = x, distance = givenPoint.Distance(x.Point) }).OrderBy(x => x.distance).ToListAsync(cancellationToken);
        if (result == null || result.Count == 0) return default;
        return result.Select(x=> (ToDomain(x.Entity), x.distance)).ToList();
    }
    

    #endregion

    
}