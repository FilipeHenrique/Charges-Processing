using Domain.Entities;

namespace Domain.Contracts.UseCases.Charges
{
    public interface IListChargesUseCase
    {
        public Task<List<Charge>> GetChargesByCPF(string cpf);
        public Task<List<Charge>> GetChargesByMonth(int month);
    }
}
