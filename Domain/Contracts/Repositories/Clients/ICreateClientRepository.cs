using Domain.Entities;

namespace Domain.Contracts.Repositories.Clients
{
    public interface ICreateClientRepository
    {
        Task CreateClient(Client client);
    }
}
