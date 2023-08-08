﻿using Domain.Contracts.Repositories.CreateClient;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories.CreateClient
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

