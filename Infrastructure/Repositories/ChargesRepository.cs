using Domain.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class ChargesRepository : IChargesRepository
    {
        private readonly IMongoCollection<Charge> collection;

        public ChargesRepository(IMongoDBContext context)
        {
            collection = context.GetCollection<Charge>("Charges");
        }

        public async Task Create(Charge charge)
        {
            await collection.InsertOneAsync(charge);
        }

        public async Task<List<Charge>> ListByCPF(string cpf)
        {
            var filter = Builders<Charge>.Filter.Eq("ClientCPF", cpf);
            var charges = await collection.Find(filter).ToListAsync();
            return charges;
        }

        public async Task<List<Charge>> ListByMonth(int month)
        {
            var currentYear = DateTime.UtcNow.Year;

            var startDate = new DateTime(currentYear, month, 1);
            var endDate = startDate.AddMonths(1);

            var filter = Builders<Charge>.Filter.And(
                Builders<Charge>.Filter.Gte("DueDate", startDate),
                Builders<Charge>.Filter.Lt("DueDate", endDate)
            );

            return await collection.Find(filter).ToListAsync();
        }
    }
}
