using gis.Domain.Common;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;

namespace gis.Domain.Entities.Events;

public record POICoordinatesChangedEvent:IDomainEvent
{
    public EntityId Id { get; init; }
    public Coordinates PrevCoordinates { get; init; }
    public Coordinates NewCoordinates { get; init; }
    
    private  POICoordinatesChangedEvent(EntityId id,Coordinates prevCoordinates,Coordinates newCoordinates)
    {
        this.Id = id;
        this.PrevCoordinates = prevCoordinates;
        this.NewCoordinates = newCoordinates;
    }

    public static POICoordinatesChangedEvent Create(EntityId id,Coordinates prevCoordinates,Coordinates newCoordinates)
    {
        return new POICoordinatesChangedEvent(id,prevCoordinates, newCoordinates);
    }
    public DateTime OccurredOn { get; }
}