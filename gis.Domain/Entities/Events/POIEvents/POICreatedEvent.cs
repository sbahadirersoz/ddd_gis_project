using gis.Domain.Common;
using gis.Domain.Entities.IDs;

namespace gis.Domain.Entities.Events;

public record POICreatedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    private  POICreatedEvent(EntityId id)
    {
        this.Id = id;
    }

    public static POICreatedEvent Create(EntityId id)
    {
        return new POICreatedEvent(id);
    }


    public DateTime OccurredOn { get; }
}