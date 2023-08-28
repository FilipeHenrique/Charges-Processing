using Clients_API.DTO;
using Domain.Clients.Entities;

namespace Clients_API.Mappers
{
    public static class ClientMapper
    {
        public static CreateClientDTO ToCreateClientDTO(Client client)
        {
            return new CreateClientDTO(client.Name, client.State, client.CPF);
        }

        public static Client ToClient(CreateClientDTO createClientDTO)
        {
            return new Client(createClientDTO.Name, createClientDTO.State, createClientDTO.CPF);
        }

        public static IEnumerable<CreateClientDTO> ListToDTO(IEnumerable<Client> clients)
        {
            return clients.Select(client => new CreateClientDTO(client.Name, client.State, client.CPF));
        }
    }
}
