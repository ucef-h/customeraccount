using System.Collections.Generic;

namespace Core
{
    public abstract class BaseEntity : Entity<string>
    {
        public List<DomainEvent> EventsHistory { get; private set; }

        private List<DomainEvent> _domainEvents;

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();


        public void AddDomainEvent(DomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ArchiveDomainEvents()
        {
            if (_domainEvents != null)
            {
                EventsHistory = EventsHistory ?? new List<DomainEvent>();
                EventsHistory.AddRange(_domainEvents);
            }

            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return this.Id == null;
        }
    }
}
