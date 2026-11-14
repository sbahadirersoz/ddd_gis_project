using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;

using Gis.Infrastructure.DB.Context;
using Gis.Infrastructure.Entity;
using Gis.Infrastructure.Mapper;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Gis.Infrastructure.Repositories;

public class LineRepository(AppDbContext context) :GenericRepository<LineAggregate,LineID,LineEntity,LineMapper>(context),ILineRepository
{
    #region Common Methods
    public WKTReader reader = new();
    private static readonly  GeometryFactory _geometryFactory =NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

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
        if (firstOrDefaultAsync == null) return null;
        return (ToDomain(firstOrDefaultAsync.entity), firstOrDefaultAsync.distance);
    }

    
    
    
    

    
    public async Task<List<LineAggregate>> FindLineStringsIncludingThisLonLat(Longitude lon, Latitude lat, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var point = _geometryFactory.CreatePoint(new Coordinate(lon.Value, lat.Value));
        var listAsync = await query.Where(x=> x.LineString.IsWithinDistance(point,0.1)).Select(x=> new {entity = x}).ToListAsync(cancellationToken: cancellationToken);
        return listAsync.Select(x=> ToDomain(x.entity)).ToList();
    }

 


    public async Task<List<(LineAggregate agg ,double distance )>> FindMostClosesCountByLonLatAsync(Longitude lon, Latitude lat, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        
        var targetPoint = _geometryFactory.CreatePoint(new Coordinate(lon.Value, lat.Value));

        
        var listAsync = await query.Select(x=> new {entity = x, distance = x.LineString.Distance(targetPoint)}).OrderBy(x=> x.distance).Take(count).ToListAsync(cancellationToken: cancellationToken);
        return listAsync.Select(x => (ToDomain(x.entity), x.distance)).ToList();


    }
    
    
    public async Task<(LineAggregate agg, double distance)?> FindClosestByLonLatAsync(Longitude lon, Latitude lat, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
    
        var targetPoint = _geometryFactory.CreatePoint(new Coordinate(lon.Value, lat.Value));
 
        var unit = await query
            .Select(x => new
            {
                Entity = x,
                Distance = x.LineString.Distance(targetPoint)
            })
            .OrderBy(x => x.Distance) 
            .FirstOrDefaultAsync(cancellationToken);
        return unit==null  ? null:(ToDomain(unit.Entity), unit.Distance);
        
    }

    public async Task<List<(LineAggregate agg ,double distance )>> FindInRangeLinesByLonLatAsync(Longitude lon, Latitude lat, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
    
        var targetPoint = _geometryFactory.CreatePoint(new Coordinate(lon.Value, lat.Value));

        var entities = await query
            .Select(x => new { Entity = x, Distance = x.LineString.Distance(targetPoint) })
            .Where(x => x.Entity.LineString.IsWithinDistance(targetPoint, distance))
            .OrderBy(x => x.Distance) 
            .ToListAsync(cancellationToken);
        return entities.Select(x => (ToDomain(x.Entity), x.Distance)).ToList();

    }

    #endregion
    
}