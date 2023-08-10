namespace Domain.Services
{
    public class CPFValidationService : ICPFValidationService
    {
        public bool IsCpf(string cpf)
        {
            int[] multiplicator1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicator2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digit;
            int sum;
            int mod;

            cpf = CPFToNumericString(cpf);

            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplicator1[i];
            mod = sum % 11;
            if (mod < 2)
                mod = 0;
            else
                mod = 11 - mod;
            digit = mod.ToString();
            tempCpf = tempCpf + digit;
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplicator2[i];
            mod = sum % 11;
            if (mod < 2)
                mod = 0;
            else
                mod = 11 - mod;
            digit = digit + mod.ToString();
            return cpf.EndsWith(digit);
        }
        public string CPFToNumericString(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            return cpf;
        }
    }
}
