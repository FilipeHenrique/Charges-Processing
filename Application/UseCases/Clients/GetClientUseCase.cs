using Domain.Contracts.Repositories;
using Domain.Contracts.UseCases.Clients;
using Domain.Entities;

namespace Application.UseCases.Clients
{
    public class GetClientUseCase : IGetClientUseCase
    {
        private readonly IClientsRepository clientsRepository;

        public GetClientUseCase(IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public async Task<Client> GetClient(string cpf)
        {
            return await clientsRepository.GetByCPF(cpf);
        }
    }
}
