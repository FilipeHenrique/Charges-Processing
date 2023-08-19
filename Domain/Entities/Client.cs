using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Client
    {   
        public Client(string name, string state, string cpf)
        {
            Name = name;
            State = state;
            CPF = cpf;
        }

        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; private set; }
        [JsonPropertyName("state")]
        public string State { get; private set; }
        [JsonPropertyName("cpf")]
        public string CPF { get; private set; }
    }
}
