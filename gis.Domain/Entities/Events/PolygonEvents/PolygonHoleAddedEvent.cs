using gis.Domain.Aggregates;
using gis.Domain.Common;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;

namespace gis.Domain.Entities.Events.PolygonEvents;

public record PolygonHoleAddedEvent : IDomainEvent

{
    public PolygonHoleAddedEvent(PolygonID id, PolygonName name, IReadOnlyList<List<CoordinateValueObject>> holes)
    {
        this.id = id;
        this.name = name;
        this.holes = holes;
    }

    public PolygonID id { get; }
    public PolygonName name { get; }
    public IReadOnlyList<List<CoordinateValueObject>> holes { get; }
    public static PolygonHoleAddedEvent Create(PolygonAggregate agg) => new(agg.Id, agg.Name, agg.Holes);
}