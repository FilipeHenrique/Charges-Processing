using Domain.Contracts.Repositories.Charges;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Charges
{
    public class CreateChargeRepository : ICreateChargeRepository
    {
        private readonly IMongoCollection<Charge> _collection;

        public CreateChargeRepository(IMongoDBContext context)
        {
            _collection = context.GetCollection<Charge>("Charges");
        }

        public async Task CreateCharege(Charge charge)
        {
            await _collection.InsertOneAsync(charge);
        }
    }
}
