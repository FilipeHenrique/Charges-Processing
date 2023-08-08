using Domain.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IGetClientRepository
    {
        Task<Client> GetClient(string cpf);
    }
}
