using gis.Domain.Aggregates;
using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;

namespace gis.Domain.Entities.Events.PolygonEvents;

public record PolygonCreatedEvent(PolygonID id ,PolygonName name ,PolygonDescription desc) : IDomainEvent

{

    public PolygonID id { get;  }
    public PolygonName name { get;  }
    public PolygonDescription desc { get;  }
    
    public static PolygonCreatedEvent Create(PolygonAggregate agg)
    => new (agg.Id,agg.Name,agg.Description);
}
    
