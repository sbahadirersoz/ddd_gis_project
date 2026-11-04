using gis.Domain.Common;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineCoordinatesChangedEvent(Guid id)
    :IDomainEvent
{
    public static LineCoordinatesChangedEvent Create(Guid LineId)
        => new(LineId);
}