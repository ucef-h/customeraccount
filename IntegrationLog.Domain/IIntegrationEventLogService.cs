using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationLog.Domain
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(string transactionId);
        Task SaveEventAsync(IntegrationEvent @event, string transactionId);
        Task MarkEventAsPublishedAsync(string eventId);
        Task MarkEventAsInProgressAsync(string eventId);
        Task MarkEventAsFailedAsync(string eventId);
    }
}
