using gis.Domain.Common;
using gis.Domain.Entities.Information.Line;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineNameChangedEvent(LineName lineName,LineName prevLine,Guid LineId):IDomainEvent
{
    public static LineNameChangedEvent Create(LineName lineName,LineName prevLine,Guid LineId)
    => new(lineName,prevLine,LineId);
}