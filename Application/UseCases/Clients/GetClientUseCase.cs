using Domain.Contracts.Repositories.Clients;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
{
    public class GetClientUseCase : IGetClientUseCase
    {
        private readonly IGetClientRepository _getClientRepository;

        public GetClientUseCase(IGetClientRepository getClientRepository)
        {
            _getClientRepository = getClientRepository;
        }

        public async Task<Client> GetClient(string cpf)
        {
            return await _getClientRepository.GetClient(cpf);
        }
    }
}
