using gis.Domain.Entities.Information;
using gis.InfrastructureLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gis.InfrastructureLayer.DB.Config;

public class PoiAggregateConfig:IEntityTypeConfiguration<PoiEntity>
{
    public void Configure(EntityTypeBuilder<PoiEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("Id");
            
        builder.Property(x => x.Point)
            .HasColumnType("geography(Point, 4326)")
            .HasColumnName("Location")
            .IsRequired();
        builder.Property(x => x.Wkt)
            .HasColumnName("Wkt")
            .HasColumnType("text")
            .IsRequired();
        builder.Property(x => x.Desc)
            .HasColumnName("Desc")
            .HasColumnType("text");
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasComment("   ACTIVE =0,\n    INACTIVE =1,\n    SOFT_DELETED =2")
            .HasDefaultValue(0)
            .HasColumnType("int");

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Point).HasMethod("GIST");
        builder.HasQueryFilter(x => x.Status != (int)POIStatus.SOFT_DELETED);
    }
}