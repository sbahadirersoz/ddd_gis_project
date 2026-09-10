using gis.Domain.Common;
using gis.Domain.Entities.Coord;


namespace gis.Domain.Entities.Events;

public record POICoordinatesChangedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    public CoordinateValueObject PrevCoordinateValueObject { get; init; }
    public CoordinateValueObject NewCoordinateValueObject { get; init; }
    
    private  POICoordinatesChangedEvent(EntityId id,CoordinateValueObject prevCoordinateValueObject,CoordinateValueObject newCoordinateValueObject)
    {
        this.Id = id;
        this.PrevCoordinateValueObject = prevCoordinateValueObject;
        this.NewCoordinateValueObject = newCoordinateValueObject;
    }

    public static POICoordinatesChangedEvent Create(EntityId id,CoordinateValueObject prevCoordinateValueObject,CoordinateValueObject newCoordinateValueObject)
    {
        return new POICoordinatesChangedEvent(id,prevCoordinateValueObject, newCoordinateValueObject);
    }
    public DateTime OccurredOn { get; }
}