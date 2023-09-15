using Clients_API.Controllers;
using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Clients.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Unit_Tests.Controllers
{
    public class ClientsControllerUnitTests
    {
        private readonly Mock<ICPFHandler> mockCPFHandler;
        private readonly Mock<IRepository<Client>> mockRepository;
        private readonly ClientsController controller;
        private readonly string validCPF, validCPFToNumericString, validCPF2, invalidCPF;


        public ClientsControllerUnitTests()
        {
            mockCPFHandler = new Mock<ICPFHandler>();
            mockRepository = new Mock<IRepository<Client>>();
            controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);
            validCPF = "960.747.590-90";
            validCPFToNumericString = "96074759090";
            validCPF2 = "183.610.120-10";
            invalidCPF = "960.747.590-91";
        }

        [Fact]
        public void CreateClient_ValidDTO_ReturnsCreatedResult()
        {
            // Arrange
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            var clientDTO = new ClientDTO("Carlos", "RJ", validCPF);

            // Act
            var result = controller.CreateClient(clientDTO);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void CreateClient_InvalidCPF_ReturnsBadRequest()
        {
            // Arrange
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(false);
            var clientDTO = new ClientDTO("Carlos", "RJ", invalidCPF);

            // Act
            var result = controller.CreateClient(clientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateClient_DuplicateCPF_ReturnsBadRequest()
        {
            // Arrange
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

            // Act
            var result = controller.CreateClient(clientDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("CPF already exists.", badRequestResult.Value);
        }

        [Fact]
        public void GetClient_IsValidCPF_ReturnsClient()
        {
            // Arrange
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

            var clientDTO = ClientMapper.ToClientDTO(expectedClient);

            // Act
            var result = controller.GetClient(validCPF);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var outputDTO = Assert.IsType<ClientDTO>(okResult.Value);
            Assert.Equal(clientDTO.CPF, outputDTO.CPF);
        }

        [Fact]
        public void GetClient_WithInvalidCpf_ReturnsBadRequest()
        {
            // Arrange
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(false);

            // Act
            var result = controller.GetClient(validCPF);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid CPF.", badRequestResult.Value);
        }

        [Fact]
        public void GetClient_ClientNotFound_ReturnsNotFound()
        {
            // Arrange
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns(validCPFToNumericString);
            mockRepository.Setup(repository => repository.Get()).Returns(new List<Client> { }.AsQueryable());

            // Act
            var result = controller.GetClient(validCPF);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Client not found.", notFoundResult.Value);
        }

        [Fact]
        public async void GetAll_ReturnsClients()
        {
            // Arrange
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

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var clientsAsync = Assert.IsAssignableFrom<IAsyncEnumerable<ClientDTO>>(okResult.Value);
            var clientsSync = await clientsAsync.ToListAsync();
            Assert.Equal(clients.Count, clientsSync.Count);
        }
    }
}
