using gis.Domain.Common;
using gis.Domain.Entities.Information;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineStatusChangedEvent(LineStatus Current, LineStatus prev, Guid idValue):IDomainEvent
{
    public static LineStatusChangedEvent Create(LineStatus Current, LineStatus prev, Guid idValue)
    => new(Current,prev,idValue);
}