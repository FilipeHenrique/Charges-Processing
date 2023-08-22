using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
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
