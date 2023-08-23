using Domain.Entities;

namespace Domain.Contracts.UseCases.Clients
{
    public interface ICreateClientUseCase
    {
        void CreateClient(Client client);
    }
}
