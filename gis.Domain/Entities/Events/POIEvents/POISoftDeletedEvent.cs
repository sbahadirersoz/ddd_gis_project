using gis.Domain.Common;
using gis.Domain.Entities.IDs;

namespace gis.Domain.Entities.Events;

public record POISoftDeletedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    
    private  POISoftDeletedEvent(EntityId id)
    {
        this.Id = id;
    }

    public static POISoftDeletedEvent Create(EntityId id)
    {
        return new POISoftDeletedEvent(id);
    }
    public DateTime OccurredOn { get; }
}