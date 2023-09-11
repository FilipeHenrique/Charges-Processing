namespace Charges_Processing_Job
{
    public class ApiUrlsConfig
    {
        public string ClientsApiUrl { get; set; }
        public string ChargesApiUrl { get; set; }

        public ApiUrlsConfig()
        {
            ClientsApiUrl = "http://localhost:7085/clients";
            ChargesApiUrl = "http://localhost:7289/charges";
        }
    }
}
