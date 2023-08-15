using Domain.Contracts.Repositories.Charges;
using Domain.Contracts.UseCases.Charges;
using Domain.Entities;

namespace Application.UseCases.Charges
{
    public class CreateChargeUseCase : ICreateChargeUseCase
    {
        private readonly ICreateChargeRepository _createChargeRepository;
        public CreateChargeUseCase(ICreateChargeRepository createChargeRepository) { 
            _createChargeRepository = createChargeRepository;
        }

        public void CreateCharge(Charge charge)
        {
            _createChargeRepository.CreateCharege(charge);
        }
    }
}
