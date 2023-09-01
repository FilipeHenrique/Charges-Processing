using Domain.Clients.Entities;
using Domain.Clients.Interfaces;
using Domain.Clients.Interfaces.Repositories;
using Domain.Clients.Interfaces.UseCases;

namespace Domain.Clients.UseCases
{
    public class GetClientUseCase : IGetClientUseCase
    {
        private readonly IRepository<Client> clientsRepository;

        public GetClientUseCase(IRepository<Client> clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public Client GetByCPF(string cpf)
        {
            return clientsRepository.Get().FirstOrDefault(client => client.CPF == cpf);
        }
    }
}
