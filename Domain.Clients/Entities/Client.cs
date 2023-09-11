using System.Text.Json.Serialization;

namespace Domain.Clients.Entities
{
    public class Client
    {
        // Default constructor for EF In memory Mapping
        public Client()
        {
            
        }

        public Client(string name, string state, string cpf)
        {
            Name = name;
            State = state;
            CPF = cpf;
        }
        public Guid Id { get; set; }
        public string Name { get; init; }
        public string State { get; init; }
        public string CPF { get; init; }
    }
}
