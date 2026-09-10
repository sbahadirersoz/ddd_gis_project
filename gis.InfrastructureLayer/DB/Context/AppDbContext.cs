using System.Reflection;
using gis.InfrastructureLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace gis.InfrastructureLayer.DB.Context;

public class AppDbContext:DbContext
{



    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<LineEntity> Line_Table { get; set; }
    public DbSet<PoiEntity> Poi_Table { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


}