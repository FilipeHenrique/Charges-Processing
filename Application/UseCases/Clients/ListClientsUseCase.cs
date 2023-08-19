using Domain.Contracts.Repositories.Clients;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
{
    public class ListClientsUseCase : IListClientsUseCase
    {
        private readonly IListClientsRpository _listClientsRepository;

        public ListClientsUseCase(IListClientsRpository listClientsRepository)
        {
            _listClientsRepository = listClientsRepository;
        }

        public async Task<List<Client>> ListClients()
        {
            return await _listClientsRepository.ListClients();
        }
    }
}
