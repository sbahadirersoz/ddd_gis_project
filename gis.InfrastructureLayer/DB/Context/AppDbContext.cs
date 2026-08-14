using gis.InfrastructureLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace gis.InfrastructureLayer.DB.Context;

public class AppDbContext:DbContext
{


    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    

    public DbSet<PoiEntity> Poi_Table { get; set; }
}