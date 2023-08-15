using Domain.Entities;

namespace Domain.Contracts.Repositories.Charges
{
    public interface IGetChargesRepository
    {
        public Task<List<Charge>> GetChargesByCPF(string cpf);
        public Task<List<Charge>> GetChargesByMonth(int month);
    }
}
