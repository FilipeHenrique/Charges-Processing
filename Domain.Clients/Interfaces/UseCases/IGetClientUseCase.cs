using Domain.Clients.Entities;

namespace Domain.Clients.Interfaces.UseCases
{
    public interface IGetClientUseCase
    {
        Client GetByCPF(string cpf);
    }
}
