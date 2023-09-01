using Domain.Clients.Entities;
using Domain.Clients.Interfaces;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class GetAllClientsUseCase : IGetAllClientsUseCase
    {
        private readonly IRepository<Client> clientsRepository;

        public GetAllClientsUseCase(IRepository<Client> clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public async IAsyncEnumerable<Client> GetAll()
        {
            var clients = clientsRepository.GetAll();

            await foreach (var client in clients)
            {
                yield return client;
            }
        }
    }
}
