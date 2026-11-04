using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;

namespace gis.Domain.Entities.Events;

public record POIDescChangedEvent:IDomainEvent
{

    public EntityId Id { get; init; }
    public PointDescription PrevPointDescription { get; init; }
    public PointDescription NewPointDescription { get; init; }

    private POIDescChangedEvent(EntityId id, PointDescription prevPointDescription, PointDescription newPointDescription)
    {
        Id = id;
        PrevPointDescription = prevPointDescription;
        NewPointDescription = newPointDescription;
    }
    public static POIDescChangedEvent Create(EntityId id, PointDescription prevPointDescription,
        PointDescription newPointDescription)
        => new(id, prevPointDescription, newPointDescription);
}