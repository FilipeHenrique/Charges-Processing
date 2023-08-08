using Domain.Entities;

namespace Domain.Contracts.Repositories.CreateClient
{
    public interface ICreateClientRepository
    {
        Task CreateClient(Client client);
    }
}
