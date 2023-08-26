﻿using Domain.Clients.Entities;
using Domain.Clients.Interfaces.Repositories;
using Infrastructure.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IMongoCollection<Client> collection;

        public ClientsRepository(IMongoDBContext context)
        {
            collection = context.GetCollection<Client>("Clients");
        }

        public async Task Create(Client client)
        {
            await collection.InsertOneAsync(client);
        }

        public async Task<Client> GetByCPF(string cpf)
        {
            var filter = Builders<Client>.Filter.Eq(client => client.CPF, cpf);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Client>> FindAll()
        {
            return await collection.Find(new BsonDocument()).ToListAsync();
        }

    }
}
