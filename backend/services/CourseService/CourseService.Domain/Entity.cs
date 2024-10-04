using CourseService.Domain.Events;

namespace CourseService.Domain
{
    public abstract class Entity
    {
        private List<DomainEvent> _domainEvents = [];
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.Add(domainEvent);
        }

        protected void AddUniqueDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.RemoveAll(e => e.GetType() == domainEvent.GetType());
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents ??= [];
            _domainEvents.Clear();
        }
    }
}
