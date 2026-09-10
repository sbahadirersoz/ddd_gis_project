using gis.Domain.Aggregates;
using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;

namespace gis.Domain.Entities.Events.PolygonEvents;

public record PolygonStatusUpdatedEvent:IDomainEvent
{
    public PolygonName name { get;  }
    public PolygonID id { get;  }

    private PolygonStatusUpdatedEvent(PolygonName name, PolygonID id)
    {
        this.name = name;
        this.id = id;
    }

    public static PolygonStatusUpdatedEvent Create(PolygonAggregate agg) => new(agg.Name, agg.Id);
}