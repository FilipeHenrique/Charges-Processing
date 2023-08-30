using Domain.Clients.Entities;
using Domain.Clients.Interfaces.Repositories;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class GetClientUseCase : IGetClientUseCase
    {
        private readonly IClientsRepository clientsRepository;

        public GetClientUseCase(IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public async Task<Client> GetByCPF(string cpf)
        {
            return await clientsRepository.GetByCPF(cpf);
        }
    }
}
