using Domain.Charges.Entities;

namespace Domain.Charges.Interfaces.UseCases
{
    public interface IListChargesUseCase
    {
        public IAsyncEnumerable<Charge> GetChargesByCPF(string cpf);
        public IAsyncEnumerable<Charge> GetChargesByMonth(int month);
    }
}
