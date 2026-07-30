namespace gis.Domain.Common;

public abstract class AggregateRoot<TId> where TId : EntityId
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

}