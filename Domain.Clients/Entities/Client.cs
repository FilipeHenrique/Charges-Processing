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
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("state")]
        public string State { get; init; }
        [JsonPropertyName("cpf")]
        public string CPF { get; init; }
    }
}
