using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationLog.Domain
{
    public interface IIntegrationEventLogRepository : IEntityRepository<IntegrationEventLogEntry, string>
    {
        Task<IntegrationEventLogEntry> FindAsync(string eventId);
        Task UpdateAsync(IntegrationEventLogEntry eventLogEntry);
        Task InsertAsync(IntegrationEventLogEntry eventLogEntry);
        Task<IEnumerable<IntegrationEventLogEntry>> FindNonPublishedByTransactionAsync(string transactionId);
    }
}
