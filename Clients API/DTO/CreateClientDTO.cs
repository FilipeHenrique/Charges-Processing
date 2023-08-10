using System.ComponentModel.DataAnnotations;

namespace Clients_API.DTO
{
    public class CreateClientDTO
    {
        public CreateClientDTO(string name, string state, string cpf)
        {
            Name = name;
            State = state;
            CPF = cpf;
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string CPF { get; set; }
    }
}
