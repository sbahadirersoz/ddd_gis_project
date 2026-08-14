using gis.Domain.Common;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Events;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Aggregates;

public class POIAggregate : AggregateRoot<PointID>,IEquatable<POIAggregate>
{
    public CoordinateValueObject CoordinateValueObject { get; private set; }
    public PointDescription? PointDesc { get; private set; }
    public PointName PointName { get; private set; }
    public POIStatus Status { get; private set; } = POIStatus.ACTIVE;

    #region Factory Method and private constructor

    internal static Result<POIAggregate> createFromPointId(
        CoordinateValueObject coords,
        PointDescription pointDesc, PointName pointName, PointID id)
    {
        var aggregate = new POIAggregate
        (
            id,coords,
            pointDesc, pointName
        );
        aggregate.AddDomainEvent(PointOfInterestCreatedEvent.Create(id));
        return Result<POIAggregate>.Success(aggregate);
    }

    private POIAggregate(PointID id, CoordinateValueObject coordinateValueObject, PointDescription? pointDesc, PointName pointName) :
        base(id)
    {
        CoordinateValueObject = coordinateValueObject;
        PointDesc = pointDesc ?? PointDescription.FromString(null).Value;
        PointName = pointName;
    }  private POIAggregate(POIAggregate aggregate):base(aggregate.Id)
    {
        CoordinateValueObject = aggregate.CoordinateValueObject;
        PointDesc = aggregate.PointDesc;
        Status = aggregate.Status;
        PointName = aggregate.PointName;
    }

    internal static Result<POIAggregate> create(
        CoordinateValueObject coordinateValueObject,
        PointDescription? pointDesc,
        PointName pointName
    )
    {
        var aggregate = new POIAggregate(
            PointID.New(),
            coordinateValueObject, pointDesc, pointName);

        aggregate.AddDomainEvent(PointOfInterestCreatedEvent.Create(aggregate.Id));

        return Result<POIAggregate>.Success(aggregate);
    }

    #endregion

    public CoordinateValueObject GetCoordinates() => CoordinateValueObject;
    public PointDescription GetPointDesc() => PointDesc;
    public PointName GetPointName() => PointName;

    internal void ChangePointDesc(PointDescription newPointDesc)
    {
        var prevPointDesc = PointDesc;
        PointDesc = newPointDesc;
        AddDomainEvent(POIPointDescChangedEvent.Create(this.Id, prevPointDesc, newPointDesc));
    }

    internal Result ChangePointName(PointName newPointName)
    {
        var prevPointName = GetPointName();
        PointName = newPointName;
        AddDomainEvent(POIPointNameChangedEvent.Create(this.Id, prevPointName, newPointName));
        return Result.Success();
    }

    internal Result ChangeCoordinates(CoordinateValueObject newCoordinateValueObject)
    {
        if (CoordinateValueObject.Equals(newCoordinateValueObject))
        {
            return Result.Failure(DomainErrors.POIErrors.Coordinate.SAME_VALUE_PROVIDED);
        }

        if (newCoordinateValueObject == null)
        {
            return Result.Failure(DomainErrors.POIErrors.Coordinate.BAD_CREDENTIALS_FOR_COORDINATES);
        }

        var prevCoordinates = GetCoordinates();
        CoordinateValueObject = newCoordinateValueObject;
        AddDomainEvent(POICoordinatesChangedEvent.Create(this.Id, prevCoordinates, newCoordinateValueObject));
        return Result.Success();
    }

    internal void ChangeStatus(POIStatus newStatus)
    {
        AddDomainEvent(POIStatusChangedEvent.Create(this.Id, Status, newStatus));
        Status = newStatus;
    }

    
    
    /// <summary>
    /// Internal olduğu için encapsulation konusunda iyi fakat
    /// rich domain model tarafında pointservice disinda  cagirilamadığı için kötü fakat okey bi tradeoff
    /// </summary>
    internal Result SoftDelete()
    {
        AddDomainEvent(POISoftDeletedEvent.Create(Id));
        Status = POIStatus.SOFT_DELETED;
        return Result.Success();
    }


    public bool Equals(POIAggregate? other)
    {
        throw new NotImplementedException();
    }
}