using Domain.Entities;

namespace Domain.Contracts.UseCases.Clients
{
    public interface IListClientsUseCase
    {
        public Task<List<Client>> ListClients();
    }
}
