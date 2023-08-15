using System;
using System.ComponentModel.DataAnnotations;

namespace Charges_API.DTO
{
    public class CreateChargeDTO
    {
        public CreateChargeDTO(float value, DateTime dueDate, string clientCPF)
        {
            Value = value;
            DueDate = dueDate;
            ClientCPF = clientCPF;
        }

        [Required(ErrorMessage = "The dueDate field is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid dueDate format.")]
        public DateTime DueDate { get; set; }
        [Required]
        public string ClientCPF { get; set; }
        [Required]
        public float Value { get; set; }

    }
}
