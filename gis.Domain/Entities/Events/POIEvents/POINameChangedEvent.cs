using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;

namespace gis.Domain.Entities.Events;

public record POINameChangedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    public PointName PrevName { get; init; }
    public PointName NewName { get; init; }
    private  POINameChangedEvent(EntityId id,PointName prevName,PointName newName)
    {
        this.Id = id;
        this.PrevName = prevName;
        this.NewName = newName;
    }

    public static POINameChangedEvent Create(EntityId id,PointName prevName,PointName newName)
    {
        return new POINameChangedEvent(id,prevName,newName);
    }


    public DateTime OccurredOn { get; }
}