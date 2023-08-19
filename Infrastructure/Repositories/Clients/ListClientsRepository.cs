using Domain.Contracts.Repositories.Clients;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Clients
{
    public class ListClientsRepository : IListClientsRpository
    {
        private readonly IMongoCollection<Client> _collection;

        public ListClientsRepository(IMongoDBContext context)
        {
            _collection = context.GetCollection<Client>("Clients");
        }
        public async Task<List<Client>> ListClients()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}