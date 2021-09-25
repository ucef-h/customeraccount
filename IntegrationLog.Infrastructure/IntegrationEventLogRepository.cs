using Core;
using IntegrationLog.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationLog.Infrastructure
{
    public class IntegrationEventLogRepository : EntityRepository<IntegrationEventLogEntry, string>, IIntegrationEventLogRepository
    {

        private readonly IMongoCollection<IntegrationEventLogEntry> _logEntry;

        public IntegrationEventLogRepository(IMongoClient client)
        {
            _logEntry = client.GetDatabase("integrationDB").GetCollection<IntegrationEventLogEntry>(EntityName);
        }

        public override string EntityName => "integration-log";


        public async Task<IntegrationEventLogEntry> FindAsync(string eventId)
        {
            var entity = await _logEntry
                .Find(filter => filter.Id.Equals(eventId))
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> FindNonPublishedByTransactionAsync(string transactionId)
        {
            var entityList = await _logEntry
               .Find(filter => filter.TransactionId.Equals(transactionId) || filter.State.Equals(EventStateEnum.NotPublished))
               .ToListAsync();
            return entityList;
        }

        public async Task InsertAsync(IntegrationEventLogEntry eventLogEntry)
        {
            await _logEntry.InsertOneAsync(eventLogEntry);
        }

        public async Task UpdateAsync(IntegrationEventLogEntry eventLogEntry)
        {
            PreUpdateEntity(eventLogEntry);
            await _logEntry.ReplaceOneAsync(filter => filter.Id.Equals(eventLogEntry.Id), eventLogEntry);
        }
    }
}
