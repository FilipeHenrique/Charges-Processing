using Domain.Entities;

namespace Domain.Contracts.UseCases.Charges
{
    public interface IGetChargesUseCase
    {
        public Task<List<Charge>> GetChargesByCPF(string cpf);
        public Task<List<Charge>> GetChargesByMonth(int month);
    }
}
