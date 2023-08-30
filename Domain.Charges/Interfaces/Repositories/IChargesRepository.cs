using Domain.Charges.Entities;

namespace Domain.Charges.Interfaces.Repositories
{
    public interface IChargesRepository
    {
        Task Create(Charge charge);
        IAsyncEnumerable<Charge> GetByCPF(string cpf);
        IAsyncEnumerable<Charge> GetByMonth(int month);
    }
}
