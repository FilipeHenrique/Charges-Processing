using Amazon.Runtime.Internal;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Quartz;
using System.Text.Json;

namespace Charges_Processing_Job
{
    public class ChargesProcessingJob : IJob
    {
        private readonly ILogger<ChargesProcessingJob> _logger;

        static HttpClient httpClient = new HttpClient();

        public ChargesProcessingJob(ILogger<ChargesProcessingJob> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                HttpResponseMessage clientsResponse = await httpClient.GetAsync("http://localhost:7085/clients");


                if (clientsResponse.IsSuccessStatusCode)
                {
                    string responseBody = await clientsResponse.Content.ReadAsStringAsync();
                    IAsyncEnumerable<Client>? clients = JsonSerializer.Deserialize<IAsyncEnumerable<Client>>(responseBody);

                    await foreach (Client client in clients)
                    {
                        string extractedDigits = $"{client.CPF[0]}{client.CPF[1]}{client.CPF[9]}{client.CPF[10]}";
                        float chargeValue = float.Parse(extractedDigits);

                        DateTime currentDate = DateTime.Now;
                        DateTime oneMonthFromNow = currentDate.AddMonths(1);

                        Charge charge = new Charge(chargeValue, oneMonthFromNow, client.CPF);

                        var content = new StringContent(JsonSerializer.Serialize(charge), System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage chargeResponse = await httpClient.PostAsync("http://localhost:7289/charges", content);

                        _logger.LogInformation($"{chargeResponse}");
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ERROR: {ex.Message}");
                throw new HttpRequestException($"Request failed with status code: {ex.Message}");
            }
        }
    }
}