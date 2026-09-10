using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.InfrastructureLayer.DB.Context;
using gis.InfrastructureLayer.Entity;
using gis.InfrastructureLayer.Mapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace gis.InfrastructureLayer.Repositories;

public class LineRepository(AppDbContext context) :GenericRepository<LineAggregate,LineID,LineEntity,LineMapper>(context),ILineRepository
{
    #region Common Methods
    public WKTReader reader = new();

    public async Task<LineAggregate?> FindByIdAsync(LineID id, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x => x.Id == id.Value).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return entity == null ? null : ToDomain(entity);
    }

    public Task<bool> IsLineNameExistsAsync(LineName pointName, CancellationToken cancellationToken = default)
    {
        var query = Table.AsQueryable();
        var anyAsync = query.Where(x=> x.Name == pointName.Value).AnyAsync(cancellationToken);
        return anyAsync;
    }

    public async Task UpdateAsync(LineAggregate entity, CancellationToken cancellationToken = default)
    {
        var query = Table.AsQueryable();
        var tableEntity = await query.FirstOrDefaultAsync(x=> x.Id == entity.Id.Value, cancellationToken: cancellationToken);
        if (tableEntity == null)
        
        if (!tableEntity.Name.Equals(entity.LineName.Value)) tableEntity.ChangeLineName(entity.LineName.Value);
        if (!tableEntity.Name.Equals(entity.LineDescription.Value)) tableEntity.ChangeLineDesc(entity.LineDescription.Value);
        if (!tableEntity.Status.Equals((int)entity.LineStatus)) tableEntity.ChangeLineStatus((int)entity.LineStatus);
        if (!tableEntity.Wkt.Equals(entity.WKT.Value)) tableEntity.ChangeLineString(entity.Coordinates);
        
    }

    public async Task<LineAggregate> FindByNameAsync(LineName name, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ?  Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x=> x.Name.Equals(name.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);  
        return entity == null ? null : ToDomain(entity);
    }

    #endregion

    #region Distance Methods

    


    public async    Task<List<(LineAggregate  entity,double distance)>> FindNearbySameEntityByGivenIdAsync(LineID entityId, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable():Table.AsNoTracking();
        
        var entity = await query.Where(x => x.Id.Equals(entityId.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var list =  await 
            query.Where(x=> x.Id!=entity.Id && entity.LineString.IsWithinDistance(x.LineString,distance))
                .Select(x=>  new { entity = x , distance = entity.LineString.Distance(x.LineString) })
            .OrderBy(x=> x.distance).ToListAsync(cancellationToken: cancellationToken);

        if (list == null) return default;
        return list.Select(x => (ToDomain(x.entity), x.distance)).ToList();
    }

    public async Task<List<(LineAggregate entity, double distance)>> FindMostClosesCountByIdAsync(LineID entityId, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable():Table.AsNoTracking();
        
        var entity = await query.Where(x => x.Id.Equals(entityId.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var listAsync = await query.Where(x=> !x.Id.Equals(entity.Id))
            .Select(x=> new{ entity = x,distance = entity.LineString.Distance(x.LineString) })
            .OrderBy(x=> x.distance)
            .Take(count).ToListAsync(cancellationToken);
        if (listAsync == null) return default;
        return listAsync.Select(x => (ToDomain(x.entity), x.distance)).ToList();
    }

    public async Task<(LineAggregate entity, double distance)?> FindClosestEntityByGivenIdAsync(LineID entityId, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable():Table.AsNoTracking();
        
        var entity = await query.Where(x => x.Id.Equals(entityId.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var firstOrDefaultAsync = await query.Where(x=> !x.Id.Equals(entity.Id))
            .Select(x=> new { entity = x, distance = entity.LineString.Distance(x.LineString) })
            .OrderBy(x=> x.distance)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (firstOrDefaultAsync == null) return new ValueTuple<LineAggregate, double>();
        return (ToDomain(firstOrDefaultAsync.entity), firstOrDefaultAsync.distance);
    }

    
    public async Task<(LineAggregate agg, double distance)> FindClosestLineByWktAsync(WellKnownText wellKnownText, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        
        
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        
        var targetLine = (LineString)reader.Read(wellKnownText.Value);
        if (targetLine is not LineString geom)
        {
            return new ValueTuple<LineAggregate, double>(); 
        }

        targetLine.SRID = 4326;
        var lineEntity = await query.Where(x => x.LineString == targetLine)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (lineEntity == null) return default;



        var result = await query.Where(x => x.LineString != targetLine)
            .Select(x => new { entity = x, distance = targetLine.Distance(x.LineString) })
            .OrderBy(x => x.distance)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return result == null ? default : ( ToDomain(result.entity), result.distance );
    }

    public async Task<List<(LineAggregate agg, double distance)>> FindInRangeLinesByWktAsync(WellKnownText wellKnownText, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        
        var targetLine = (LineString)reader.Read(wellKnownText.Value);
        
            if (targetLine is not LineString geom)
            {
                return new List<(LineAggregate agg, double distance)>();
            }
        targetLine.SRID = 4326;
        var lineEntity = await query.Where(x => x.LineString == targetLine)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (lineEntity == null) return new List<(LineAggregate agg, double distance)>();
        var result = await query.Where
                (x => x.LineString != targetLine && targetLine.IsWithinDistance(x.LineString, distance))
            
            .Select (x => new { entity = x, distance = targetLine.Distance(x.LineString) })
            .OrderBy(x => x.distance)
            .ToListAsync(cancellationToken: cancellationToken);
        if (result == null) return new List<(LineAggregate agg, double distance)>();
        return result.Select(x => (ToDomain(x.entity), x.distance)).ToList();
    }

    public async Task<List<(LineAggregate agg, double distance)>> FindMostClosesCountByWktAsync(
        WellKnownText wellKnownText,
        int count,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        var reader = new WKTReader();
        var parsedGeom = reader.Read(wellKnownText.Value);
        if (parsedGeom is not LineString targetLine)
            return new List<(LineAggregate agg, double distance)>();

        targetLine.SRID = 4326; 

        var lineEntity = await query
            .FirstOrDefaultAsync(x => x.Wkt == wellKnownText.Value, cancellationToken);

        if (lineEntity == null)
            return new List<(LineAggregate agg, double distance)>();

        var list = await query
            .Where(x => x.Id != lineEntity.Id)
            .Select(x => new {
                Entity = x,
                Distance = x.LineString.Distance(targetLine)
            })
            .OrderBy(x => x.Distance)
            .Take(count)
            .ToListAsync(cancellationToken);

        return list.Select(x => (agg: ToDomain(x.Entity), distance: x.Distance)).ToList();
    }

    public async Task<(LineAggregate agg, double distance)> FindClosestLineByNameAsync(LineName name, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x => x.Name.Equals(name.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity == null) return new ValueTuple<LineAggregate, double>();
        var closest = query.Where(x=> !x.Name.Equals(name.Value))
            .Select(x=> new {entity = x , distance = entity.LineString.Distance(x.LineString) })
            .OrderBy(x=> x.distance)
            .FirstOrDefault();
        return closest == null ? new ValueTuple<LineAggregate, double>() : (ToDomain(closest.entity), closest.distance);
    }

    public async Task<List<(LineAggregate agg, double distance)>> FindInRangeLinesByLineNameAsync(LineName name, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x => x.Name.Equals(name.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity == null) return new List<(LineAggregate agg, double distance)>();
        var list = query.Where(x=> !entity.Id.Equals(x.Id) && entity.LineString.IsWithinDistance(x.LineString,distance))
            .Select(x=> new {entity = x , distance = entity.LineString.Distance(x.LineString) })
            .OrderBy(x=> x.distance)
            .ToList();
        return 
            (list.Select(x => (ToDomain(x.entity), x.distance)).ToList());
    }

    public async Task<List<(LineAggregate agg, double distance)>> FindMostClosesCountByLineNameAsync(LineName name, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x => x.Name.Equals(name.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity == null) return new List<(LineAggregate agg, double distance)>();
        var list = query.Where(x => !entity.Id.Equals(x.Id))
            .Select
            (x => new
                {
                    entity = x,
                    distance = entity.LineString.Distance(x.LineString)
                }
            )
            .OrderBy(x => x.distance)
            .Take(count)
            .ToList();
        return 
            (list.Select(x => (ToDomain(x.entity), x.distance)).ToList());
    }
    
    
    
    
    
    
    /// <summary>
    /// Lonlat
    /// </summary>
    /// <param name="lon"></param>
    /// <param name="lat"></param>
    /// <param name="count"></param>
    /// <param name="tracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    
    public async Task<List<LineAggregate>> FindLineStringsIncludingThisLonLat(Longitude lon, Latitude lat, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var coordinate = new Coordinate(lon.Value, lat.Value);
        var listAsync = await query.Where(x=> x.LineString.Coordinates.Contains(coordinate)).Select(x=> new {entity = x}).ToListAsync(cancellationToken: cancellationToken);
        return listAsync.Select(x=> ToDomain(x.entity)).ToList();
    }

    public async Task<List<(LineAggregate agg ,double distance )>> FindMostClosesCountByLonLatAsync(Longitude lon, Latitude lat, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var coordinate = new Coordinate(lon.Value, lat.Value);
        
        var listAsync = await query.Select(x=> new {entity = x, distance = coordinate.Distance(x.LineString.Coordinate)}).OrderBy(x=> x.distance).Take(count).ToListAsync(cancellationToken: cancellationToken);
        return listAsync.Select(x => (ToDomain(x.entity), x.distance)).ToList();


    }

    public async Task<List<(LineAggregate agg ,double distance )>> FindInRangeLinesByLonLatAsync(Longitude lon, Latitude lat, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
    
        var targetPoint = new Point(lon.Value, lat.Value) { SRID = 4326 };

        var entities = await query
                .Select(x => new
        {
            Entity = x,
            Distance = x.LineString.Distance(targetPoint)
        })
            .Where(x => x.Entity.LineString.IsWithinDistance(targetPoint, distance))
            .OrderBy(x => x.Distance) //
            .ToListAsync(cancellationToken);

        return entities.Select(x=> (ToDomain(x.Entity),x.Distance)).ToList();
    }

    public async Task<(LineAggregate agg, double distance)> FindClosestByLonLatAsync(Longitude lon, Latitude lat, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
    
        var targetPoint = new Point(lon.Value, lat.Value) { SRID = 4326 };
        
        var unit = await query
            .Select(x => new
            {
                Entity = x,
                Distance = x.LineString.Distance(targetPoint)
            })
            .OrderBy(x => x.Distance) //
            .FirstOrDefaultAsync(cancellationToken);
        return (ToDomain(unit.Entity), unit.Distance);
        
    }

    #endregion
    
}