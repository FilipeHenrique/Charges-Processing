using Domain.Charges.Entities;
using Domain.Charges.Interfaces.Repositories;
using Domain.Charges.Interfaces.UseCases;

namespace Domain.Charges.UseCases
{
    public class ListChargesUseCase : IListChargesUseCase
    {
        private readonly IChargesRepository chargesRepository;
        public ListChargesUseCase(IChargesRepository chargesRepository)
        {
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
