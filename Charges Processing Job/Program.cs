using Microsoft.Extensions.Configuration;
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
                    IConfiguration configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory()) 
                        .AddJsonFile("appsettings.json")
                        .Build();
                    services.AddSingleton(configuration);

                    services.AddJob();

                    services.AddHttpClient("ClientsAPI", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri(configuration["ApiUrlsConfig:ClientsApiUrl"]);
                    });

                    services.AddHttpClient("ChargesAPI", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri(configuration["ApiUrlsConfig:ChargesApiUrl"]);
                    });
                });
    }
}