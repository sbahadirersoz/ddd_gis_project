using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;

namespace gis.Domain.Entities.Events;

public record POIStatusChangedEvent : IDomainEvent
{
    public EntityId Id { get; init; }
    public POIStatus PrevStatus { get; init; }
    public POIStatus NewStatus { get; init; }

    private POIStatusChangedEvent(EntityId id, POIStatus prevStatus, POIStatus newStatus)
    {
        this.Id = id;
        this.PrevStatus = prevStatus;
        this.NewStatus = newStatus;
    }

    public static POIStatusChangedEvent Create(EntityId id, POIStatus prevStatus, POIStatus newStatus)
    {
        return new POIStatusChangedEvent(id, prevStatus, newStatus);
    }
    public DateTime OccurredOn { get; }
}