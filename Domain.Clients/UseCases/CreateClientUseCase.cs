using Domain.Clients.Entities;
using Domain.Clients.Interfaces;
using Domain.Clients.Interfaces.Repositories;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class CreateClientUseCase : ICreateClientUseCase
    {
        private readonly IRepository<Client> clientsRepository;

        public CreateClientUseCase(IRepository<Client> clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public void Create(Client client)
        {
            clientsRepository.Create(client);
        }
    }
}