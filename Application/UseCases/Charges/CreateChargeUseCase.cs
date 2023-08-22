using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases.Charges;
using Domain.Entities;

namespace Application.UseCases.Charges
{
    public class CreateChargeUseCase : ICreateChargeUseCase
    {
        private readonly IChargesRepository chargesRepository;
        public CreateChargeUseCase(IChargesRepository chargesRepository) {
            this.chargesRepository = chargesRepository;
        }

        public void CreateCharge(Charge charge)
        {
            chargesRepository.Create(charge);
        }
    }
}
