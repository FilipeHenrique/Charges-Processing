using Domain.Charges.Entities;
using Domain.Charges.Interfaces.Repositories;
using Domain.Charges.Interfaces.UseCases;
using System.Runtime.InteropServices;

namespace Domain.Charges.UseCases
{
    public class ListChargesUseCase : IListChargesUseCase
    {
        private readonly IChargesRepository chargesRepository;
        public ListChargesUseCase(IChargesRepository chargesRepository)
        {
            this.chargesRepository = chargesRepository;
        }
        public async IAsyncEnumerable<Charge> GetChargesByCPF(string cpf)
        {
            var charges = chargesRepository.ListByCPF(cpf);

            await foreach (var charge in charges)
            {
                yield return charge;
            }            
        }
        public async IAsyncEnumerable<Charge> GetChargesByMonth(int month)
        {
            var charges = chargesRepository.ListByMonth(month);

            await foreach (var charge in charges)
            {
                yield return charge;
            }
        }
    }
}
