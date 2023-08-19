using Domain.Entities;

namespace Domain.Contracts.Repositories.Clients
{
    public interface IGetClientRepository
    {
        Task<Client> GetClient(string cpf);
    }
}
