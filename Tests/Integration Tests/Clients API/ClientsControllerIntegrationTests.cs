using Clients_API.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net.Http.Json;
using Charges_Processing_Job;

namespace Tests.Integration_Tests.Clients_API
{
    public class ClientsControllerIntegrationTests
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly HttpClient httpClient;
        private readonly string validCPF, validCPF2, invalidCPF;

        public ClientsControllerIntegrationTests() { 
           factory = new WebApplicationFactory<Program>();
           httpClient = factory.CreateClient();
           validCPF = "960.747.590-90";
           validCPF2 = "183.610.120-10";
           invalidCPF = "960.747.590-91";
        }

        [Fact]
        public async Task Test_CreateClient_ValidInput_ReturnsCreated()
        {
            // Arrange
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);

            // Act
            var response = await httpClient.PostAsJsonAsync("/Clients", clientDTO);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetClient_ValidCPF_ReturnsOk()
        {
            // Arrange
            // Create a client before get
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);
            await httpClient.PostAsJsonAsync("/Clients", clientDTO);

            // Act
            var response = await httpClient.GetAsync($"/Clients/{validCPF}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetClient_InvalidCPF_ReturnsBadRequest()
        {
            // Act
            var response = await httpClient.GetAsync($"/Clients/{invalidCPF}");

            // Assert
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

            // Act
            var response = await httpClient.GetAsync("/Clients");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            var clients = JsonSerializer.Deserialize<List<ClientDTO>>(content);
            Assert.NotNull(clients);
            Assert.Equal(2, clients.Count);
        }
    }
}
