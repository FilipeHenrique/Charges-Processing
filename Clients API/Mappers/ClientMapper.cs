using Clients_API.DTO;
using Domain.Entities;

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
    }
}
