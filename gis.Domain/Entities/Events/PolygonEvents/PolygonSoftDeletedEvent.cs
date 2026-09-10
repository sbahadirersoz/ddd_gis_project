using gis.Domain.Aggregates;
using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;

namespace gis.Domain.Entities.Events.PolygonEvents;

public record PolygonSoftDeletedEvent(PolygonID id,PolygonName name):IDomainEvent
{
    public PolygonID Id { get; }
    public PolygonName Name { get; }
    public static PolygonSoftDeletedEvent Create( PolygonAggregate agg) => new(agg.Id,agg.Name);
}