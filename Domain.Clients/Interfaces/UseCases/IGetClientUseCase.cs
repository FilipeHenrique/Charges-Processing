using Domain.Clients.Entities;

namespace Domain.Clients.Interfaces.UseCases
{
    public interface IGetClientUseCase
    {
        Task<Client> GetByCPF(string cpf);
    }
}
