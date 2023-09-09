namespace Infrastructure.Services
{
    public interface ICPFHandler
    {
        bool IsCpf(string cpf);
        string CPFToNumericString(string cpf);
    }
}
