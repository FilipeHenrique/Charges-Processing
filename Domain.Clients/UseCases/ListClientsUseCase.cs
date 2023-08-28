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

        public async IAsyncEnumerable<Client> ListClients()
        {
            var clients =  clientsRepository.FindAll();

            await foreach (var client in clients)
            {
                yield return client;
            }
        }
    }
}
