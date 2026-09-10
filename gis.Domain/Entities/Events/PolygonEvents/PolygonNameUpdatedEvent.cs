using gis.Domain.Aggregates;
using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;

namespace gis.Domain.Entities.Events.PolygonEvents;

public record PolygonNameUpdatedEvent(PolygonName name, PolygonID id):IDomainEvent

{
    public static PolygonNameUpdatedEvent Create(PolygonAggregate agg) => new(agg.Name, agg.Id);
}