using Domain.Entities;
namespace Domain.Contracts.Repositories
{
    public interface IClientsRepository
    {
        Task Create(Client client);
        Task<Client> GetByCPF(string cpf);
        public Task<List<Client>> FindAll();

    }
}
