using gis.Domain.Common;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineWktChangedEvent(Guid LineId)
    :IDomainEvent
{
    public static LineWktChangedEvent Create(Guid LineId)
        => new(LineId);
}