using Domain.Contracts.Repositories.Clients;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Clients
{
    public class CreateClientRepository : ICreateClientRepository
    {
        private readonly IMongoCollection<Client> _collection;

        public CreateClientRepository(IMongoDBContext context)
        {
            _collection = context.GetCollection<Client>("Clients");
        }

        public async Task CreateClient(Client client)
        {
            await _collection.InsertOneAsync(client);
        }

    }
}

