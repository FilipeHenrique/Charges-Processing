namespace Domain.Services
{
    public interface ICPFValidationService
    {
        bool IsCpf(string cpf);
        string CPFToNumericString(string cpf);
    }
}
