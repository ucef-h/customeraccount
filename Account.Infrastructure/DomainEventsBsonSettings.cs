using Account.Domain.Events;
using Core;
using MongoDB.Bson.Serialization;

namespace Account.Infrastructure
{
    public static class DomainEventsBsonSettings
    {
        public static void AddBsonSettings()
        {
            BsonClassMap.RegisterClassMap<DomainEvent>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.SetDiscriminatorIsRequired(true);
                cm.AddKnownType(typeof(AccountCreatedEvent));
                cm.AddKnownType(typeof(DepositEvent));
                cm.AddKnownType(typeof(WithdrawalEvent));
            });
        }
    }
}