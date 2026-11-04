using gis.Domain.Entities.Information;

using Gis.Infrastructure.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gis.Infrastructure.DB.Config;

public class LineAggregateConfig:IEntityTypeConfiguration<LineEntity>
{
    public void Configure(EntityTypeBuilder<LineEntity> builder)
    {

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id");

        builder.Property(x => x.LineString)
            .HasColumnType("geography(LineString, 4326)")
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
        builder.HasIndex(x => x.LineString).HasMethod("GIST");
        builder.HasQueryFilter(x => x.Status != (int)LineStatus.SOFT_DELETED);
    }
}