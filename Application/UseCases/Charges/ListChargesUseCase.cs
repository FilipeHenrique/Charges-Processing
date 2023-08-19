using Domain.Contracts.Repositories.Charges;
using Domain.Contracts.UseCases.Charges;
using Domain.Entities;

namespace Application.UseCases.Charges
{
    public class ListChargesUseCase : IListChargesUseCase
    {
        private readonly IListChargesRepository _getChargesRepository;
        public ListChargesUseCase(IListChargesRepository getChargesRepository) {
            _getChargesRepository = getChargesRepository;
        }
        public Task<List<Charge>> GetChargesByCPF(string cpf)
        {
            return _getChargesRepository.GetChargesByCPF(cpf);
        }
        public Task<List<Charge>> GetChargesByMonth(int month)
        {
            return _getChargesRepository.GetChargesByMonth(month);
        }
    }
}
