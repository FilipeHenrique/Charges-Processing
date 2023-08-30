using Domain.Charges.Entities;
using Domain.Charges.Interfaces.Repositories;
using Domain.Charges.Interfaces.UseCases;
using System.Runtime.InteropServices;

namespace Domain.Charges.UseCases
{
    public class GetChargesUseCase : IGetChargesUseCase
    {
        private readonly IChargesRepository chargesRepository;

        public GetChargesUseCase(IChargesRepository chargesRepository)
        {
            this.chargesRepository = chargesRepository;
        }

        public async IAsyncEnumerable<Charge> GetByCPF(string cpf)
        {
            var charges = chargesRepository.GetByCPF(cpf);

            await foreach (var charge in charges)
            {
                yield return charge;
            }            
        }

        public async IAsyncEnumerable<Charge> GetByMonth(int month)
        {
            var charges = chargesRepository.GetByMonth(month);

            await foreach (var charge in charges)
            {
                yield return charge;
            }
        }
    }
}
