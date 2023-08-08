using Domain.Entities;

namespace Domain.Contracts.UseCases
{
    public interface ICreateClientUseCase
    {
        void CreateClient(Client client);
    }
}
