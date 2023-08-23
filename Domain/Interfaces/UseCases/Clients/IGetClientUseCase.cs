using Domain.Entities;

namespace Domain.Contracts.UseCases.Clients
{
    public interface IGetClientUseCase
    {
        Task<Client> GetClient(string cpf);
    }
}
