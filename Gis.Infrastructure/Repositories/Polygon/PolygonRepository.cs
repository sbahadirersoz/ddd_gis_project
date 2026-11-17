using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Repositories;
using Gis.Infrastructure.DB.Context;
using Gis.Infrastructure.Entity;
using Gis.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Gis.Infrastructure.Repositories;

public class PolygonRepository(AppDbContext context) :GenericRepository<PolygonAggregate,PolygonID,PolygonEntity,PolygonMapper>(context),IPolygonRepository
{
    #region Crud

    public async Task<PolygonAggregate?> FindByIdAsync(PolygonID id, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        var firstOrDefaultAsync = await query.FirstOrDefaultAsync(x => x.Id.Equals(id.Value), cancellationToken: cancellationToken);
        return (firstOrDefaultAsync == null)  ?  
        null:
        ToDomain(firstOrDefaultAsync);
    }

    public async Task<bool> IsPolygonNameExistsAsync(PolygonName polygonName, CancellationToken cancellationToken = default)
    {
        var Any =  Table.AnyAsync(x => x.Name.Equals(polygonName.Value), cancellationToken: cancellationToken);
        return  await Any;
    }

    #endregion
}
