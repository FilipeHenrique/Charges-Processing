using Domain.Charges.Entities;

namespace Domain.Charges.Interfaces.UseCases
{
    public interface ICreateChargeUseCase
    {
        public void Create(Charge charge);
    }
}
