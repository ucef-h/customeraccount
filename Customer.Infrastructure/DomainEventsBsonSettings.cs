using Core;
using Customer.Domain.Events;
using MongoDB.Bson.Serialization;

namespace Customer.Infrastructure
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
                cm.AddKnownType(typeof(CustomerCreatedEvent));
            });
        }
    }
}