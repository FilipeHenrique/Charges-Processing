using Clients_API.Controllers;
using Clients_API.DTO;
using Domain.Clients.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clients_API.Tests
{
    public class ClientsControllerUnitTests
    {
        private readonly Mock<ICPFHandler> mockCPFHandler = new Mock<ICPFHandler>();
        private readonly Mock<IRepository<Client>> mockRepository = new Mock<IRepository<Client>>();
        private readonly ClientsController controller;
        private const string validCPF = "960.747.590-90";
        private const string validCPF2 = "183.610.120-10";
        private const string invalidCPF = "960.747.590-91";
        private const string validCPFToNumericString = "96074759090";


        public ClientsControllerUnitTests()
        {
            controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);
        }

        [Fact]
        public void CreateClient_ReturnsCreatedResult()
        {
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);

            var result = controller.CreateClient(clientDTO);

            var createdResult = Assert.IsType<CreatedResult>(result);
            var createdClient = Assert.IsType<Client>(createdResult.Value);
        }

        [Fact]
        public void CreateClient_InvalidCPF_ReturnsBadRequest()
        {
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(false);
            var clientDTO = new ClientDTO("Carlos", "RJ", invalidCPF);

            var result = controller.CreateClient(clientDTO);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateClient_DuplicateCPF_ReturnsBadRequest()
        {
            var expectedClient = new Client
            {
                Name = "Carlos",
                State = "RJ",
                CPF = validCPFToNumericString
            };

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns(validCPFToNumericString);
            mockRepository.Setup(repository => repository.Get())
                .Returns(new List<Client> { expectedClient }.AsQueryable());

            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);

            var result = controller.CreateClient(clientDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("CPF already exists.", badRequestResult.Value);
        }

        [Fact]
        public void GetClient_IsValidCPF_ReturnsClient()
        {
            var expectedClient = new Client
            {
                Name = "Carlos",
                State = "RJ",
                CPF = validCPFToNumericString
            };

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns(validCPFToNumericString);
            mockRepository.Setup(repository => repository.Get())
                .Returns(new List<Client> { expectedClient }.AsQueryable());


            var result = controller.GetClient(validCPF);

            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var outputClient = Assert.IsType<Client>(okResult.Value);
            Assert.Equal(expectedClient.CPF, outputClient.CPF);
        }

        [Fact]
        public void GetClient_InvalidCpf_ReturnsBadRequest()
        {
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(false);

            var result = controller.GetClient(validCPF);

            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid CPF.", badRequestResult.Value);
        }

        [Fact]
        public void GetClient_ClientNotFound_ReturnsNotFound()
        {
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns(validCPFToNumericString);
            mockRepository.Setup(repository => repository.Get()).Returns(new List<Client> { }.AsQueryable());

            var result = controller.GetClient(validCPF);

            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Client not found.", notFoundResult.Value);
        }

        [Fact]
        public async void GetAll_ReturnsClients()
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

            mockRepository.Setup(repository => repository.GetAll()).Returns(clients.ToAsyncEnumerable);

            var result = controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var clientsAsync = Assert.IsAssignableFrom<IAsyncEnumerable<Client>>(okResult.Value);
            var clientsSync = await clientsAsync.ToListAsync();
            Assert.Equal(clients.Count, clientsSync.Count);
        }
    }
}
