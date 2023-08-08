using Domain.Entities;

namespace Domain.Contracts.UseCases
{
    public interface IGetClientUseCase
    {
        Task<Client> GetClient(string cpf);
    }
}
