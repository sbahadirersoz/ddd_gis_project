using gis.Domain.Common;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineCreatedEvent:IDomainEvent
{
    
    public EntityId Id { get; init; }
    private  LineCreatedEvent(EntityId id)
    {
        this.Id = id;
    }

    public static LineCreatedEvent Create(EntityId id)
    {
        return new LineCreatedEvent(id);
    }


    public DateTime OccurredOn { get; }
}
