using Domain.Charges.Entities;
using Domain.Clients.Entities;
using Quartz;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Charges_Processing_Job
{
    public class ChargesProcessingJob : IJob
    {

        private readonly IHttpClientFactory httpClientFactory;

        public ChargesProcessingJob(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var clients = GetClients();
            var chargesByState = CreateCharges(clients);
            return Report(chargesByState);
        }

        public async IAsyncEnumerable<Client> GetClients()
        {
            var httpClient = httpClientFactory.CreateClient("ClientsAPI");
            var clientsResponse = await httpClient.GetAsync("/Clients");
            clientsResponse.EnsureSuccessStatusCode();

            var clients = await clientsResponse.Content.ReadFromJsonAsync<IAsyncEnumerable<Client>>();

            await foreach (var client in clients)
            {
                yield return client;
            }
        }

        public async IAsyncEnumerable<(string state, float value)> CreateCharges(IAsyncEnumerable<Client> clients)
        {
            var httpClient = httpClientFactory.CreateClient("ChargesAPI");

            await foreach (var client in clients)
            {
                var extractedDigits = $"{client.CPF[0]}{client.CPF[1]}{client.CPF[9]}{client.CPF[10]}";
                var chargeValue = float.Parse(extractedDigits);

                var currentDate = DateTime.Now;
                var oneMonthFromNow = currentDate.AddMonths(1);

                var charge = new Charge(chargeValue, oneMonthFromNow, client.CPF);
                var content = new StringContent(JsonSerializer.Serialize(charge), Encoding.UTF8, "application/json");

                var chargeResponse = await httpClient.PostAsync("/Charges", content);
                chargeResponse.EnsureSuccessStatusCode();

                yield return (client.State, chargeValue);
            }

        }

        public async Task Report(IAsyncEnumerable<(string state, float value)> chargesByState)
        {
            var totalChargesByState = chargesByState
                .GroupBy(charge => charge.state)
                .Select(group => new
                {
                    state = group.Key,
                    total = group.SumAsync(charge => charge.value)
                })
                .OrderBy(charge => charge.state);

            var currentDirectory = Directory.GetCurrentDirectory();
            var desiredDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..")); // removes \bin\Debug\net6.0
            var filePath = Path.Combine(desiredDirectory, "Report.txt");
            using var writer = new StreamWriter(filePath, false);
            await foreach (var charge in totalChargesByState)
            {
                writer.WriteLine($"State: {charge.state}, Total: {charge.total}");
            }
        }
    }
}