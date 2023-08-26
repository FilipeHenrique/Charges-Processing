using Domain.Charges.Entities;
using Domain.Clients.Entities;
using Microsoft.Extensions.Logging;
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
                var response = await GetClients();
                var clients = JsonSerializer.Deserialize<IAsyncEnumerable<Client>>(response);
                var chargesByState = await ProcessCharges(clients);
                MapReduce(chargesByState);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ERROR: {ex.Message}");
                throw new HttpRequestException($"Processing failed with error: {ex.Message}");
            }
        }

        private async Task<string> GetClients()
        {
            var clientsResponse = await httpClient.GetAsync("http://localhost:7085/clients");
            clientsResponse.EnsureSuccessStatusCode();
            return await clientsResponse.Content.ReadAsStringAsync();
        }

        private async Task<List<(string state, float value)>> ProcessCharges(IAsyncEnumerable<Client> clients)
        {
            List<(string state, float value)> chargesList = new List<(string, float)>();

            await foreach (Client client in clients)
            {
                var extractedDigits = $"{client.CPF[0]}{client.CPF[1]}{client.CPF[9]}{client.CPF[10]}";
                var chargeValue = float.Parse(extractedDigits);

                var currentDate = DateTime.Now;
                var oneMonthFromNow = currentDate.AddMonths(1);

                var charge = new Charge(chargeValue, oneMonthFromNow, client.CPF);

                var content = new StringContent(JsonSerializer.Serialize(charge), System.Text.Encoding.UTF8, "application/json");
                var chargeResponse = await httpClient.PostAsync("http://localhost:7289/charges", content);
                chargeResponse.EnsureSuccessStatusCode();

                chargesList.Add((client.State, chargeValue));
            }

            return chargesList;
        }

        private void MapReduce(List<(string state, float value)> chargesList)
        {
            var totalChargesByState = chargesList
                .GroupBy(charge => charge.state)
                .Select(group => new
                {
                    State = group.Key,
                    Total = group.Sum(charge => charge.value)
                })
                .OrderBy(charge => charge.State);

            foreach (var charge in totalChargesByState)
            {
                _logger.LogInformation($"{charge.State}: {charge.Total}");
            }
        }
    }
}