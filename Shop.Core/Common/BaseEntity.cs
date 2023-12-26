using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Common;
public abstract class BaseEntity
{
    private readonly List<BaseEvent> _domainEvents = new List<BaseEvent>();

    public Guid Id { get; set; }

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }
}
