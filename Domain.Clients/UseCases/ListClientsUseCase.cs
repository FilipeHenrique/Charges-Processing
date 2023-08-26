using Domain.Clients.Entities;
using Domain.Clients.Interfaces.Repositories;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class ListClientsUseCase : IListClientsUseCase
    {
        private readonly IClientsRepository clientsRepository;

        public ListClientsUseCase(IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public async Task<List<Client>> ListClients()
        {
            return await clientsRepository.FindAll();
        }
    }
}
