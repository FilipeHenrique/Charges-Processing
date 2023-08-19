using Domain.Contracts.Repositories.Charges;
using Domain.Entities;
using Infrastructure.DbContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Charges
{
    public class GetChargesRepository : IListChargesRepository
    {
        private readonly IMongoCollection<Charge> _collection;

        public GetChargesRepository(IMongoDBContext context)
        {
            _collection = context.GetCollection<Charge>("Charges");
        }
        public async Task<List<Charge>> GetChargesByCPF(string cpf)
        {
            var filter = Builders<Charge>.Filter.Eq("ClientCPF", cpf);
            var charges = await _collection.Find(filter).ToListAsync();
            return charges;
        }

        public async Task<List<Charge>> GetChargesByMonth(int month)
        {
            int currentYear = DateTime.UtcNow.Year;

            var startDate = new DateTime(currentYear, month, 1);
            var endDate = startDate.AddMonths(1);

            var filter = Builders<Charge>.Filter.And(
                Builders<Charge>.Filter.Gte("DueDate", startDate),
                Builders<Charge>.Filter.Lt("DueDate", endDate)
            );

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
