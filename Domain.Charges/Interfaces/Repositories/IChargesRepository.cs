using Domain.Charges.Entities;

namespace Domain.Charges.Interfaces.Repositories
{
    public interface IChargesRepository
    {
        Task Create(Charge charge);
        IAsyncEnumerable<Charge> ListByCPF(string cpf);
        IAsyncEnumerable<Charge> ListByMonth(int month);
    }
}
