using Clients_API.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net.Http.Json;

namespace Clients_API.Tests
{
    public class ClientsControllerIntegrationTests
    {
        private readonly WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();
        private readonly HttpClient httpClient;
        private const string validCPF = "960.747.590-90";
        private const string validCPF2 = "183.610.120-10";
        private const string invalidCPF = "960.747.590-91";

        public ClientsControllerIntegrationTests()
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Test_CreateClient_ValidInput_ReturnsCreated()
        {
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);
            var response = await httpClient.PostAsJsonAsync("/Clients", clientDTO);
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetClient_ValidCPF_ReturnsOk()
        {
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);
            await httpClient.PostAsJsonAsync("/Clients", clientDTO);

            var response = await httpClient.GetAsync($"/Clients/{validCPF}");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetClient_InvalidCPF_ReturnsBadRequest()
        {
            var response = await httpClient.GetAsync($"/Clients/{invalidCPF}");
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetAll_ReturnsOk()
        {
            // Create 2 clients before get
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);
            await httpClient.PostAsJsonAsync("/Clients", clientDTO);

            clientDTO = new ClientDTO("Lucas", "MG", validCPF2);
            await httpClient.PostAsJsonAsync("/Clients", clientDTO);

            var response = await httpClient.GetAsync("/Clients");

            var content = await response.Content.ReadAsStringAsync();
            var clients = JsonSerializer.Deserialize<List<ClientDTO>>(content);
            Assert.NotNull(clients);
            Assert.Equal(2, clients.Count);
        }
    }
}
