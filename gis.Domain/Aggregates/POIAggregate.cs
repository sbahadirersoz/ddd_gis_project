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
    public Coordinates Coordinates { get; private set; }
    public PointDescription? PointDesc { get; private set; }
    public PointName PointName { get; private set; }
    public POIStatus Status { get; private set; } = POIStatus.ACTIVE;

    #region Factory Method and private constructor

    internal static Result<POIAggregate> createFromPointId(
        Coordinates coords,
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

    private POIAggregate(PointID id, Coordinates coordinates, PointDescription? pointDesc, PointName pointName) :
        base(id)
    {
        Coordinates = coordinates;
        PointDesc = pointDesc ?? PointDescription.FromString(null).Value;
        PointName = pointName;
    }  private POIAggregate(POIAggregate aggregate):base(aggregate.Id)
    {
        Coordinates = aggregate.Coordinates;
        PointDesc = aggregate.PointDesc;
        Status = aggregate.Status;
        PointName = aggregate.PointName;
    }

    internal static Result<POIAggregate> create(
        Coordinates coordinates,
        PointDescription? pointDesc,
        PointName pointName
    )
    {
        var aggregate = new POIAggregate(
            PointID.New(),
            coordinates, pointDesc, pointName);

        aggregate.AddDomainEvent(PointOfInterestCreatedEvent.Create(aggregate.Id));

        return Result<POIAggregate>.Success(aggregate);
    }

    #endregion

    public Coordinates GetCoordinates() => Coordinates;
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

    internal Result ChangeCoordinates(Coordinates newCoordinates)
    {
        if (Coordinates.Equals(newCoordinates))
        {
            return Result.Failure(DomainErrors.POIErrors.Coordinate.SAME_VALUE_PROVIDED);
        }

        if (newCoordinates == null)
        {
            return Result.Failure(DomainErrors.POIErrors.Coordinate.BAD_CREDENTIALS_FOR_COORDINATES);
        }

        var prevCoordinates = GetCoordinates();
        Coordinates = newCoordinates;
        AddDomainEvent(POICoordinatesChangedEvent.Create(this.Id, prevCoordinates, newCoordinates));
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

    public bool CompareEquality(POIAggregate obj)
    {
        return this.PointDesc.Equals(PointDesc);
    }

    public static Result<POIAggregate> Clone(POIAggregate aggregate)
    {
        if (aggregate == null)
        {
            Result<POIAggregate>.Failure(DomainErrors.POIErrors.INVALID_PARAMETER_FOR_CLONING);
        }
        return Result<POIAggregate>.Success(new POIAggregate(aggregate));
    }

    public bool Equals(POIAggregate? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Coordinates.Equals(other.Coordinates) && Equals(PointDesc, other.PointDesc) && PointName.Equals(other.PointName) && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((POIAggregate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Coordinates, PointDesc, PointName, (int)Status);
    }
}