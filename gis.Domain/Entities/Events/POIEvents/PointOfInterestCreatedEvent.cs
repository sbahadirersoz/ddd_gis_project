using gis.Domain.Common;
using gis.Domain.Entities.IDs;

namespace gis.Domain.Entities.Events;

public record PointOfInterestCreatedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    private  PointOfInterestCreatedEvent(EntityId id)
    {
        this.Id = id;
    }

    public static PointOfInterestCreatedEvent Create(EntityId id)
    {
        return new PointOfInterestCreatedEvent(id);
    }


    public DateTime OccurredOn { get; }
}