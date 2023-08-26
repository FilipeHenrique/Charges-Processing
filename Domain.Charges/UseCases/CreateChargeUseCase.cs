using Domain.Charges.Entities;
using Domain.Charges.Interfaces.Repositories;
using Domain.Charges.Interfaces.UseCases;

namespace Domain.Charges.UseCases
{
    public class CreateChargeUseCase : ICreateChargeUseCase
    {
        private readonly IChargesRepository chargesRepository;
        public CreateChargeUseCase(IChargesRepository chargesRepository)
        {
            this.chargesRepository = chargesRepository;
        }

        public void CreateCharge(Charge charge)
        {
            chargesRepository.Create(charge);
        }
    }
}
