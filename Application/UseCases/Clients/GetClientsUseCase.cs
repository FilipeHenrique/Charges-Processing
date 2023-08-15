using Domain.Contracts.Repositories.Clients;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
{
    public class GetClientsUseCase : IGetClientsUseCase
    {
        private readonly IGetClientsRepository _getClientRepository;

        public GetClientsUseCase(IGetClientsRepository getClientRepository)
        {
            _getClientRepository = getClientRepository;
        }

        public async Task<Client> GetClient(string cpf)
        {
            return await _getClientRepository.GetClient(cpf);
        }
    }
}
