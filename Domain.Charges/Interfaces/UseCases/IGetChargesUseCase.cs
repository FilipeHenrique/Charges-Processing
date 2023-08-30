using Domain.Charges.Entities;

namespace Domain.Charges.Interfaces.UseCases
{
    public interface IGetChargesUseCase
    {
        public IAsyncEnumerable<Charge> GetByCPF(string cpf);
        public IAsyncEnumerable<Charge> GetByMonth(int month);
    }
}
