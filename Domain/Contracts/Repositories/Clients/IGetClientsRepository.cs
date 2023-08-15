using Domain.Entities;

namespace Domain.Contracts.Repositories.Clients
{
    public interface IGetClientsRepository
    {
        Task<Client> GetClient(string cpf);
    }
}
