using MongoDB.Driver;

namespace Infrastructure.DbContext
{
    public class DBContext : IDBContext
    {
        private readonly IMongoDatabase database;

        public DBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database => database;

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return database.GetCollection<T>(collectionName);
        }
    }
}