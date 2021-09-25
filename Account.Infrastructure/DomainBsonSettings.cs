using MongoDB.Bson.Serialization;

namespace Account.Infrastructure
{
    public static class DomainBsonSettings
    {
        public static void AddBsonSettings()
        {
            BsonClassMap.RegisterClassMap<Domain.Account>(cm =>
            {
                cm.AutoMap();
                cm.UnmapField(m => m.owner);
                cm.UnmapField(m => m.Balance);
            });
        }
    }
}