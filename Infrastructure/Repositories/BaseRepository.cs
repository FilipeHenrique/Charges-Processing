using Domain.Clients.Interfaces;
using Infrastructure.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> collection;

        public BaseRepository(IDBContext context, string collectionName)
        {
            collection = context.GetCollection<T>(collectionName);
        }

        public async Task Create(T entity)
        {
            await collection.InsertOneAsync(entity);
        }

        public IQueryable<T> Get()
        {
            return collection.AsQueryable();
        }

        public async IAsyncEnumerable<T> GetAll()
        {
            var entitiesCursor = await collection.FindAsync(new BsonDocument());

            while (await entitiesCursor.MoveNextAsync())
            {
                foreach (var entity in entitiesCursor.Current)
                {
                    yield return entity;
                }
            }
        }

    }
}
