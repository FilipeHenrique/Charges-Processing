using System.ComponentModel.DataAnnotations;

namespace Domain.Charges.Entities
{
    public class Charge
    {
        // Default constructor for EF In memory Mapping
        public Charge()
        {

        }

        public Charge(float value, DateTime dueDate, string clientCPF)
        {
            Value = value;
            DueDate = dueDate;
            ClientCPF = clientCPF;
        }

        public Guid Id { get; set; }
        public DateTime DueDate { get; init; }
        public string ClientCPF { get; init; }
        public float Value { get; init; }

    }
}
