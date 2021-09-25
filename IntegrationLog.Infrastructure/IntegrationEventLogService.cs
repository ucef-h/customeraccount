using Core;
using IntegrationLog.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationLog.Infrastructure
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IIntegrationEventLogRepository _repository;

        public IntegrationEventLogService(
            IIntegrationEventLogRepository repository
        )
        {
            _repository = repository;
        }


        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(string transactionId)
        {

            var result = await _repository.FindNonPublishedByTransactionAsync(transactionId);

            if (result != null && result.Any())
            {
                return result;
            }

            return new List<IntegrationEventLogEntry>();
        }

        public async Task SaveEventAsync(IntegrationEvent @event, string transactionId)
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event, transactionId);

            await _repository.InsertAsync(eventLogEntry);
        }


        public async Task MarkEventAsPublishedAsync(string eventId)
        {
            await UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public async Task MarkEventAsInProgressAsync(string eventId)
        {
            await UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public async Task MarkEventAsFailedAsync(string eventId)
        {
            await UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private async Task UpdateEventStatus(string eventId, EventStateEnum status)
        {
            var eventLogEntry = await _repository.FindAsync(eventId);
            eventLogEntry.SetStatus(status);

            if (status == EventStateEnum.InProgress)
            {
                eventLogEntry.IncrementTimeSent();
            }


            await _repository.UpdateAsync(eventLogEntry);
        }
    }
}
