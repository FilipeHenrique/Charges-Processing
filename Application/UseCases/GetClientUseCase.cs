using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases;
using Domain.Entities;

namespace Application.UseCases
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
