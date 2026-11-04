namespace gis.Domain.Common;

public abstract class AggregateRoot<TId> : IEquatable<AggregateRoot<TId>>
    where TId : EntityId
{
    public TId Id { get; private set; }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    private readonly List<IDomainEvent> _domainEvents = [];

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    // Aggregates have identity semantics: two instances are equal when they
    // are the same type and share the same Id, whatever their current state.
    public bool Equals(AggregateRoot<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetType() == other.GetType() && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as AggregateRoot<TId>);

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(AggregateRoot<TId>? left, AggregateRoot<TId>? right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(AggregateRoot<TId>? left, AggregateRoot<TId>? right)
        => !(left == right);
}
