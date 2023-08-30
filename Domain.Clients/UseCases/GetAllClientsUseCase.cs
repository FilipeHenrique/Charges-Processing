using Domain.Clients.Entities;
using Domain.Clients.Interfaces.Repositories;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class GetAllClientsUseCase : IGetAllClientsUseCase
    {
        private readonly IClientsRepository clientsRepository;

        public GetAllClientsUseCase(IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public async IAsyncEnumerable<Client> GetAll()
        {
            var clients =  clientsRepository.FindAll();

            await foreach (var client in clients)
            {
                yield return client;
            }
        }
    }
}
