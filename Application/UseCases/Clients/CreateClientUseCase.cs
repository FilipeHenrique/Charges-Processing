using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
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