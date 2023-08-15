﻿using Domain.Contracts.Repositories.Clients;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Clients
{
    public class GetClientsRepository : IGetClientsRepository
    {
        private readonly IMongoCollection<Client> _collection;

        public GetClientsRepository(IMongoDBContext context)
        {
            _collection = context.GetCollection<Client>("Clients");
        }
        public async Task<Client> GetClient(string cpf)
        {
            var filter = Builders<Client>.Filter.Eq(client => client.CPF, cpf);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}