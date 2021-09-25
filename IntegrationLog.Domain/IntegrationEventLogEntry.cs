using Core;

namespace IntegrationLog.Domain
{
    public class IntegrationEventLogEntry : Entity<string>
    {
        private IntegrationEventLogEntry() { }
        public IntegrationEventLogEntry(IntegrationEvent @event, string transactionId)
        {
            Id = @event.Id;
            CreatedDate = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            IntegrationEvent = @event;
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }

        public string EventTypeName { get; private set; }

        public IntegrationEvent IntegrationEvent { get; private set; }

        public EventStateEnum State { get; set; }

        public int TimesSent { get; set; }

        public string TransactionId { get; private set; }

        public void IncrementTimeSent()
        {
            TimesSent++;
        }

        public void SetStatus(EventStateEnum state)
        {
            this.State = state;
        }
    }
}
