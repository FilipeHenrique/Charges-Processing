using Domain.Clients.Entities;
using System.Collections.Generic;

namespace Domain.Clients.Interfaces.UseCases
{
    public interface IListClientsUseCase
    {
        public IAsyncEnumerable<Client> ListClients();
    }
}
