using Domain.Entities;
using MongoDB.Bson.Serialization;

public static class MongoDBConfig
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<Client>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            cm.MapIdMember(client => client.Id);
        });

    }
}