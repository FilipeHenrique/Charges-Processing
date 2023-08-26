using Domain.Charges.Entities;

namespace Domain.Charges.Interfaces.UseCases
{
    public interface IListChargesUseCase
    {
        public Task<List<Charge>> GetChargesByCPF(string cpf);
        public Task<List<Charge>> GetChargesByMonth(int month);
    }
}
