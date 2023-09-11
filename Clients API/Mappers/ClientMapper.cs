using Clients_API.DTO;
using Domain.Clients.Entities;

namespace Clients_API.Mappers
{
    public static class ClientMapper
    {
        public static ClientDTO ToClientDTO(Client client)
        {
            return new ClientDTO(client.Name, client.State, client.CPF);
        }

        public static Client ToClient(ClientDTO clientDTO)
        {
            return new Client(clientDTO.Name, clientDTO.State, clientDTO.CPF);
        }

        public static async IAsyncEnumerable<ClientDTO> ClientsToDTO(IAsyncEnumerable<Client> clients)
        {
            await foreach (var client in clients)
            {
                yield return new ClientDTO(client.Name, client.State, client.CPF);
            }
        }
    }
}
