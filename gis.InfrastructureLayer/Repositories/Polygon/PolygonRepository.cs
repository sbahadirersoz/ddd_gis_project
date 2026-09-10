using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Repositories;
using gis.InfrastructureLayer.Entity;
using gis.InfrastructureLayer.Mapper;
using Microsoft.EntityFrameworkCore;

namespace gis.InfrastructureLayer.Repositories;

public class PolygonRepository(DbContext context) :GenericRepository<PolygonAggregate,PolygonID,PolygonEntity,PolygonMapper>(context),IPolygonRepository
{


    #region Crud

    public async Task<PolygonAggregate?> FindByIdAsync(PolygonID id, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var firstOrDefaultAsync = await query.FirstOrDefaultAsync(x => x.Id.Equals(id.Value), cancellationToken: cancellationToken);
        return ToDomain(firstOrDefaultAsync);
    }

    public async Task<bool> IsPolygonNameExistsAsync(PolygonName polygonName, CancellationToken cancellationToken = default)
    {
        var Any =  Table.Any(x => x.Name.Equals(polygonName.Value));
        return Any;
    }

    public async Task UpdateAsync(PolygonAggregate entity, CancellationToken cancellationToken = default)
    {
        
        var query = Table.AsQueryable();
        var tableEntity = await query.FirstOrDefaultAsync(x=> x.Id == entity.Id.Value, cancellationToken: cancellationToken);
        if (tableEntity == null) 
        
        if (!tableEntity.Name.Equals(entity.Name.Value)) tableEntity.ChangePolygonName(entity.Name.Value);
        if (!tableEntity.Name.Equals(entity.Description.Value)) tableEntity.ChangePolygonDesc(entity.Description.Value);
        if (!tableEntity.Status.Equals((int)entity.Status)) tableEntity.ChangePolygonStatus((int)entity.Status);
        if (!tableEntity.Wkt.Equals(entity.Shell.wkt.Value)) tableEntity.ChangePolygonShell(entity.Shell.Coordinates);
        
    }

    public async Task<PolygonAggregate> FindByNameAsync(PolygonName name, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = Table.AsQueryable();
        var result = await query.FirstOrDefaultAsync(x=> x.Name == name.Value, cancellationToken: cancellationToken);
        if (result == null) return null;
        return ToDomain(result);
    }
    

    #endregion


    #region Distance By ID

    public async Task<List<(PolygonAggregate entity, double distance)>> FindNearbySameEntityByGivenIdAsync(PolygonID entityId, double distance, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x=> x.Id.Equals(entityId.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity  == null) return default;

        var listAsync = await query.Where(x=>x.Id!=entityId.Value && entity.Polygon.IsWithinDistance( x.Polygon, distance))
            .Select(
                x=> 
                    new
                    {
                        agg =  ToDomain(x),
                        distance = entity.Polygon.Distance(x.Polygon)
                    }
                ).OrderBy(x=> x.distance)
            
            .ToListAsync(cancellationToken: cancellationToken);
        
        return listAsync.Select( x=> (x.agg,x.distance)).ToList();
        
    }

    public async Task<List<(PolygonAggregate entity, double distance)>> FindMostClosesCountByIdAsync(PolygonID entityId, int count, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x => x.Id.Equals(entityId.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity == null) return default;
        var listAsync = await query.Where(x => x.Id != entity.Id).Select(x=>   new
                {
                    agg =  ToDomain(x),
                    distance = entity.Polygon.Distance(x.Polygon)
                }
            ).OrderBy(x=> x.distance)
            .Take(count).ToListAsync(cancellationToken);

        return listAsync.Select(x => (x.agg, x.distance)).ToList();
    }

    public async Task<(PolygonAggregate entity, double distance)?> FindClosestEntityByGivenIdAsync(PolygonID entityId, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var entity = await query.Where(x => x.Id.Equals(entityId.Value)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity == null) return default;
        var result = await query.Where(x => x.Id != entity.Id).Select(x => new
                {
                    agg = ToDomain(x),
                    distance = entity.Polygon.Distance(x.Polygon)
                }
            ).OrderBy(x => x.distance)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (result == null) return default;
        return  (result.agg, result.distance);
    }

    #endregion
}