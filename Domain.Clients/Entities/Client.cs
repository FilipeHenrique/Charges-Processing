using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Domain.Clients.Entities
{
    public class Client
    {
        public Client(string name, string state, string cpf)
        {
            Name = name;
            State = state;
            CPF = cpf;
        }
        public ObjectId Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; private set; }
        [JsonPropertyName("state")]
        public string State { get; private set; }
        [JsonPropertyName("cpf")]
        public string CPF { get; private set; }
    }
}
