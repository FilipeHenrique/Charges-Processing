using Domain.Clients.Entities;
using Domain.Clients.Interfaces.Repositories;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class CreateClientUseCase : ICreateClientUseCase
    {
        private readonly IClientsRepository clientsRepository;

        public CreateClientUseCase(IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }
        public void CreateClient(Client client)
        {
            clientsRepository.Create(client);
        }
    }
}