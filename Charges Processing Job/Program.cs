using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Charges_Processing_Job
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddJob();

                    services.AddHttpClient("ClientsAPI", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri("http://localhost:7085/clients");
                    });

                    services.AddHttpClient("ChargesAPI", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri("http://localhost:7289/charges");
                    });
                });
    }
}