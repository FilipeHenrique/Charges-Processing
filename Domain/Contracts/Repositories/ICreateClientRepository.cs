using Domain.Entities;

namespace Domain.Contracts.Repositories
{
    public interface ICreateClientRepository
    {
        Task CreateClient(Client client);
    }
}
