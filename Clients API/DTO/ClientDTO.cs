using System.ComponentModel.DataAnnotations;

namespace Clients_API.DTO
{
    public class ClientDTO
    {
        public ClientDTO(string name, string state, string cpf)
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
