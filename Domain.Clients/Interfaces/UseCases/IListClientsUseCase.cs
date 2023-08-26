using Domain.Clients.Entities;

namespace Domain.Clients.Interfaces.UseCases
{
    public interface IListClientsUseCase
    {
        public Task<List<Client>> ListClients();
    }
}
