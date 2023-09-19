using Charges_Processing_Job;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Domain.Clients.Entities;
using Domain.Charges.Entities;
using Xunit.Abstractions;

namespace End_to_End_Tests
{
    public class EndToEndTests
    {
        private readonly WebApplicationFactory<Clients_API.Program> clientsAPI = new WebApplicationFactory<Clients_API.Program>();
        private readonly WebApplicationFactory<Charges_API.Program> chargesAPI = new WebApplicationFactory<Charges_API.Program>();
        private readonly HttpClient clientsApiHttpClient, chargesApiHttpClient;

        private const string validCPF = "960.747.590-90";
        private const string validCPF2 = "183.610.120-10";

        public EndToEndTests(ITestOutputHelper output)
        {
            clientsApiHttpClient = clientsAPI.CreateClient();
            chargesApiHttpClient = chargesAPI.CreateClient();
        }

        [Fact]
        public async Task ChargesProcessing_EndToEnd()
        {
            var clients = new List<Client>
            {
                new Client {
                    Name = "Carlos",
                    State = "RJ",
                    CPF = validCPF
                },
                 new Client {
                    Name = "Lucas",
                    State = "MG",
                    CPF = validCPF2
                },
            };

            // Create Clients
            foreach(var client in clients)
            {
                await clientsApiHttpClient.PostAsJsonAsync("/Clients", client);
            }

            // Prepare Job
            var host = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<HttpClient>();
                services.AddTransient<ChargesProcessingJob>();

                services.AddHttpClient("ClientsAPI", client => client.BaseAddress = clientsApiHttpClient.BaseAddress)
                      .ConfigurePrimaryHttpMessageHandler(() => clientsAPI.Server.CreateHandler());

                services.AddHttpClient("ChargesAPI", client => client.BaseAddress = chargesApiHttpClient.BaseAddress)
                    .ConfigurePrimaryHttpMessageHandler(() => chargesAPI.Server.CreateHandler());
            })
            .Build();

            await host.StartAsync();

            try
            {
                // run Job
                var job = host.Services.GetRequiredService<ChargesProcessingJob>();
                await job.Execute(null); 
            }
            finally
            {
                await host.StopAsync();
            }

            // Get Created Charges
            var nextMonth = DateTime.Now.AddMonths(1).Month;
            var chargesResponse = await chargesApiHttpClient.GetAsync($"/Charges?month={nextMonth}");
            var charges = await chargesResponse.Content.ReadFromJsonAsync<List<Charge>>();

            Assert.Equal("9690", charges[0].Value.ToString());
            Assert.Equal("1810", charges[1].Value.ToString());

            // Get Created file
            var currentDirectory = Directory.GetCurrentDirectory();
            var desiredDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..")); // removes \bin\Debug\net6.0
            var filePath = Path.Combine(desiredDirectory, "Report.txt");
            var fileContents = await File.ReadAllLinesAsync(filePath);

            // Assert in file (ordered bottom up)
            Assert.Equal("State: MG, Total: 1810", fileContents[0]);
            Assert.Equal("State: RJ, Total: 9690", fileContents[1]);
        }

    }
}
