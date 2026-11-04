using gis.Domain.Common;
using gis.Domain.Entities.Information.Line;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineSoftDeletedEvent(Guid LineId):IDomainEvent
{
    public static LineSoftDeletedEvent Create(Guid LineId)
        => new(LineId);
}