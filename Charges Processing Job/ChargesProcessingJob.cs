﻿using Domain.Charges.Entities;
using Domain.Clients.Entities;
using Quartz;
using System.Text.Json;

namespace Charges_Processing_Job
{
    public class ChargesProcessingJob : IJob
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly ApiUrlsConfig apiUrls = new ApiUrlsConfig();

        public Task Execute(IJobExecutionContext context)
        {
            var clients = GetClients();
            var chargesByState = CreateCharges(clients);
            return Report(chargesByState);
        }

        private async IAsyncEnumerable<Client> GetClients()
        {
            var clientsResponse = await httpClient.GetAsync(apiUrls.ClientsApiUrl);
            clientsResponse.EnsureSuccessStatusCode();

            var json = await clientsResponse.Content.ReadAsStringAsync();
            var clients = JsonSerializer.Deserialize<IAsyncEnumerable<Client>>(json);

            await foreach (var client in clients)
            {
                yield return client;
            }
        }

        private async IAsyncEnumerable<(string state, float value)> CreateCharges(IAsyncEnumerable<Client> clients)
        {

            await foreach (var client in clients)
            {
                var extractedDigits = $"{client.CPF[0]}{client.CPF[1]}{client.CPF[9]}{client.CPF[10]}";
                var chargeValue = float.Parse(extractedDigits);

                var currentDate = DateTime.Now;
                var oneMonthFromNow = currentDate.AddMonths(1);

                var charge = new Charge(chargeValue, oneMonthFromNow, client.CPF);

                var content = new StringContent(JsonSerializer.Serialize(charge), System.Text.Encoding.UTF8, "application/json");
                var chargeResponse = await httpClient.PostAsync(apiUrls.ChargesApiUrl, content);
                chargeResponse.EnsureSuccessStatusCode();

                yield return (client.State, chargeValue);
            }

        }

        private async Task Report(IAsyncEnumerable<(string state, float value)> chargesByState)
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