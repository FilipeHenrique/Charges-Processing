using Domain.Clients.Entities;

namespace Domain.Clients.Interfaces.UseCases
{
    public interface ICreateClientUseCase
    {
        void CreateClient(Client client);
    }
}
