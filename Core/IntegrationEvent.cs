using System;

namespace Core
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationEvent(string id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        public string Id { get; private set; }

        public DateTime CreationDate { get; private set; }
    }


    public interface IIntegrationEvent { }
}
