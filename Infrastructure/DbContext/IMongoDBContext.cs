using MongoDB.Driver;

namespace Infrastructure.DbContext
{
    public interface IMongoDBContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> GetCollection<T>(string collectionName);

    }
}
