using Domain.Entities;

namespace Domain.Contracts.UseCases.Clients
{
    public interface IGetClientsUseCase
    {
        Task<Client> GetClient(string cpf);
    }
}
