using Domain.Entities;

namespace Domain.Contracts.UseCases.Charges
{
    public interface ICreateChargeUseCase
    {
        public void CreateCharge(Charge charge);
    }
}
