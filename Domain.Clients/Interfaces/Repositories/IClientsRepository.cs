using Domain.Clients.Entities;
namespace Domain.Clients.Interfaces.Repositories
{
    public interface IClientsRepository
    {
        Task Create(Client client);
        Task<Client> GetByCPF(string cpf);
        public Task<List<Client>> FindAll();

    }
}
