using gis.Domain.Common;
using gis.Domain.Entities.Information.Line;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineDescChangedEvent(LineDescription description,LineDescription prevDescription,Guid LineId):IDomainEvent
{
    public static LineDescChangedEvent Create(LineDescription description,LineDescription prevDescription,Guid LineId)
    => new LineDescChangedEvent(description,prevDescription,LineId);
}