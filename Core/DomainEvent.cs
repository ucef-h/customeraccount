using MediatR;
using System;

namespace Core
{
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            EventName = this.GetType().Name;
        }

        public DomainEvent(Guid id, DateTime createDate, string eventName)
        {
            Id = id;
            CreationDate = createDate;
            EventName = eventName;
        }

        public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }

        public string EventName { get; private set; }
    }

    public interface IDomainEvent : INotification { }
}
