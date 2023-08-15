using Domain.Contracts.Repositories.Clients;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
{
    public class CreateClientUseCase : ICreateClientUseCase
    {
        private readonly ICreateClientRepository _createClientRepository;

        public CreateClientUseCase(ICreateClientRepository createClientRepository)
        {
            _createClientRepository = createClientRepository;
        }
        public void CreateClient(Client client)
        {
            _createClientRepository.CreateClient(client);
        }
    }
}