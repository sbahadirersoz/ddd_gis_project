using gis.Domain.Entities.Information.Polygon;

using Gis.Infrastructure.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gis.Infrastructure.DB.Config;

public class PolygonAggregateConfig : IEntityTypeConfiguration<PolygonEntity>
{
    public void Configure(EntityTypeBuilder<PolygonEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id");

        // holes are stored as the polygon's interior rings
        builder.Property(x => x.Polygon)
            .HasColumnType("geography(Polygon, 4326)")
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
        builder.HasIndex(x => x.Name).IsUnique().HasFilter($"\"status\" <> {(int)PolygonStatus.SOFT_DELETED}");

        builder.HasIndex(x => x.Polygon).HasMethod("GIST");
        builder.HasQueryFilter(x => x.Status != (int)PolygonStatus.SOFT_DELETED);
    }
}
