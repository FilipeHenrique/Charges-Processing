using MongoDB.Bson;

namespace Domain.Entities
{
    public class Charge
    {
        public Charge(float value, DateTime dueDate, string clientCPF)
        {
            Value = value;
            DueDate = dueDate;
            ClientCPF = clientCPF;
        }
        public ObjectId Id { get; set; }
        public DateTime DueDate { get; private set; }
        public string ClientCPF { get; private set; }
        public float Value { get; private set; }

    }
}
