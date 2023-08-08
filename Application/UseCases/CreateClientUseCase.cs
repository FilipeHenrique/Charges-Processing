using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases;
using Domain.Entities;

namespace Application.UseCases
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