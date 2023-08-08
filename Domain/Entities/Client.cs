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
        public string? Id { get; set; }
        public string Name { get; private set; }
        public string State { get; private set; }
        public string CPF { get; private set; }

    }
}
