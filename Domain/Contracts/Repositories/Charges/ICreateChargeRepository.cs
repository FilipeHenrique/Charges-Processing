using Domain.Entities;

namespace Domain.Contracts.Repositories.Charges
{
    public interface ICreateChargeRepository
    {
        Task CreateCharege(Charge charge);
    }
}
