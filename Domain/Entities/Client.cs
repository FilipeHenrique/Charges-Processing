using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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
        public string Name { get; private set; }
        public string State { get; private set; }
        public string CPF { get; private set; }
    }
}
