using MongoDB.Driver;

namespace Infrastructure.DbContext
{
    public interface IDBContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> GetCollection<T>(string collectionName);

    }
}
