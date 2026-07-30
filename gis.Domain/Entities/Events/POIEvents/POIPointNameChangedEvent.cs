using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;

namespace gis.Domain.Entities.Events;

public record POIPointNameChangedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    public PointName PrevName { get; init; }
    public PointName NewName { get; init; }
    private  POIPointNameChangedEvent(EntityId id,PointName prevName,PointName newName)
    {
        this.Id = id;
        this.PrevName = prevName;
        this.NewName = newName;
    }

    public static POIPointNameChangedEvent Create(EntityId id,PointName prevName,PointName newName)
    {
        return new POIPointNameChangedEvent(id,prevName,newName);
    }


    public DateTime OccurredOn { get; }
}