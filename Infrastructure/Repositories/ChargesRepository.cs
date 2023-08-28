using Domain.Charges.Entities;
using Domain.Charges.Interfaces.Repositories;
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

        public async IAsyncEnumerable<Charge> ListByCPF(string cpf)
        {
            var filter = Builders<Charge>.Filter.Eq("ClientCPF", cpf);
            var charges = await collection.FindAsync(filter);

            while (await charges.MoveNextAsync())
            {
                foreach (var charge in charges.Current)
                {
                    yield return charge;
                }
            }
        }

        public async IAsyncEnumerable<Charge> ListByMonth(int month)
        {
            var currentYear = DateTime.UtcNow.Year;

            var startDate = new DateTime(currentYear, month, 1);
            var endDate = startDate.AddMonths(1);

            var filter = Builders<Charge>.Filter.And(
                Builders<Charge>.Filter.Gte("DueDate", startDate),
                Builders<Charge>.Filter.Lt("DueDate", endDate)
            );

            var charges = await collection.FindAsync(filter);

            while (await charges.MoveNextAsync())
            {
                foreach (var charge in charges.Current)
                {
                    yield return charge;
                }
            }
        }
    }
}
