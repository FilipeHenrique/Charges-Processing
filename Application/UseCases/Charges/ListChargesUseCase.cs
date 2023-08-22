using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases.Charges;
using Domain.Entities;

namespace Application.UseCases.Charges
{
    public class ListChargesUseCase : IListChargesUseCase
    {
        private readonly IChargesRepository chargesRepository;
        public ListChargesUseCase(IChargesRepository chargesRepository) {
            this.chargesRepository = chargesRepository;
        }
        public Task<List<Charge>> GetChargesByCPF(string cpf)
        {
            return chargesRepository.ListByCPF(cpf);
        }
        public Task<List<Charge>> GetChargesByMonth(int month)
        {
            return chargesRepository.ListByMonth(month);
        }
    }
}
