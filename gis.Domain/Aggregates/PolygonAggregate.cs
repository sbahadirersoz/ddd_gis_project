using gis.Domain.BusinessRules.PolygonRules;
using gis.Domain.BusinessRules.PolygonRules.Holes;
using gis.Domain.BusinessRules.PolygonRules.Status;
using gis.Domain.Common;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.Events.PolygonEvents;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.ResultPattern;

namespace gis.Domain.Aggregates;

public class PolygonAggregate : AggregateRoot<PolygonID>, IEquatable<PolygonAggregate>
{
    public PolygonShell Shell { get; private set; }

    public IReadOnlyList<List<CoordinateValueObject>> Holes => _holesList.AsReadOnly();
    private readonly List<List<CoordinateValueObject>> _holesList = new();
    public PolygonName Name { get; private set; }
    public PolygonDescription Description { get; private set; }
    public PolygonStatus Status { get; private set; } = PolygonStatus.ACTIVE;

    private PolygonAggregate(PolygonID id, PolygonShell shell, PolygonName name, PolygonDescription description,
        PolygonStatus status) : base(id)
    {
        Shell = shell;
        Name = name;
        Description = description;
        Status = status;
    }

    private PolygonAggregate(PolygonID id) : base(id)
    {
    }

    public static Result<PolygonAggregate>Create( PolygonShell shell, PolygonName name, PolygonDescription description)
    {
        var polygonId = PolygonID.New();
        var agg = new PolygonAggregate(polygonId, shell, name, description, PolygonStatus.ACTIVE);
        agg.AddDomainEvent(PolygonCreatedEvent.Create(agg));
        return Result<PolygonAggregate>.Success(agg);
    }

    #region Update Helpers

    internal Result UpdateName(PolygonName newName)
    {
        Name = newName;
        
        AddDomainEvent(PolygonNameUpdatedEvent.Create(this));
        return Result.Success();
    }

    internal void UpdateDescription(PolygonDescription desc)
    {
        Description = desc;
    }

    internal Result UpdateStatus(PolygonStatus status)
    {
        var checkNotSoftDeletedRuleSet = CheckRuleSet(new PolygonMustBeNotSoftDeleted(this));
        if (checkNotSoftDeletedRuleSet.IsFailure) return Result.Failure(checkNotSoftDeletedRuleSet.Error);
        
        var checkRuleSet = CheckRuleSet(new PolygonStatusShouldntBeSame(status,this));
        if (checkRuleSet.IsFailure) return  Result.Failure(checkRuleSet.Error);
        Status = status;
        return Result.Success();
    }

    internal Result UpdateHoles(List<CoordinateValueObject> holes)
    {
        _holesList.Clear();
        _holesList.AddRange(holes);
        AddDomainEvent(PolygonHolesUpdatedEvent.Create(this));
        return Result.Success();
    }

    internal Result SoftDelete()
    {
        var checkRuleSet = CheckRuleSet(new PolygonMustBeNotSoftDeleted(this));
        if (checkRuleSet.IsFailure) return Result.Failure(checkRuleSet.Error);
        Status = PolygonStatus.SOFT_DELETED;
        AddDomainEvent(PolygonSoftDeletedEvent.Create(this));
        return Result.Success();
    } 
    
    internal Result AddHole(List<CoordinateValueObject> hole)
    {
        var checkRuleSet = CheckRuleSet(new PolygonMustBeNotSoftDeleted(this));
        if (checkRuleSet.IsFailure) return Result.Failure(checkRuleSet.Error);
        _holesList.Add(hole);
        AddDomainEvent(PolygonHoleAddedEvent.Create(this));
        return Result.Success();
    }

    #endregion

    public bool Equals(PolygonAggregate? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _holesList.SequenceEqual(other._holesList) && Shell.Equals(other.Shell) && Name.Equals(other.Name) &&
               Description.Equals(other.Description) && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PolygonAggregate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_holesList, Shell, Name, Description, (int)Status);
    }

    private Result CheckRuleSet(IPolygonRule rule)
    {
        var result = rule.Execute();
        return (result.IsFailure) ? Result.Failure(result.Error) : Result.Success();

    }
}