﻿using Clients_API.Controllers;
using Clients_API.DTO;
using Clients_API.Mappers;
using Domain.Clients.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Unit_Tests.Controllers
{
    public class ClientsControllerTest
    {
        [Fact]
        public void CreateClient_ValidDTO_ReturnsCreatedResult()
        {
            // Arrange
            var mockCPFHandler = new Mock<ICPFHandler>();
            var mockRepository = new Mock<IRepository<Client>>();

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);

            var clientDTO = new ClientDTO("Carlos", "RJ", "960.747.590-90");
            var controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);

            // Act
            var result = controller.CreateClient(clientDTO);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void CreateClient_InvalidCPF_ReturnsBadRequest()
        {
            // Arrange
            var mockCPFHandler = new Mock<ICPFHandler>();
            var mockRepository = new Mock<IRepository<Client>>();

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(false);

            var clientDTO = new ClientDTO("Carlos", "RJ", "960.747.590-91");
            var controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);

            // Act
            var result = controller.CreateClient(clientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateClient_DuplicateCPF_ReturnsBadRequest()
        {
            // Arrange
            var mockCPFHandler = new Mock<ICPFHandler>();
            var mockRepository = new Mock<IRepository<Client>>();

            var expectedClient = new Client
            {
                Name = "Carlos",
                State = "RJ",
                CPF = "96074759090"
            };

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns("96074759090");
            mockRepository.Setup(repository => repository.Get())
                .Returns(new List<Client> { expectedClient }.AsQueryable());

            var clientDTO = new ClientDTO("Carlos", "RJ", "960.747.590-90");
            var controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);

            // Act
            var result = controller.CreateClient(clientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetClient_IsValidCPF_ReturnsSearchedClient()
        {
            // Arrange
            var mockCPFHandler = new Mock<ICPFHandler>();
            var mockRepository = new Mock<IRepository<Client>>();

            var expectedClient = new Client
            {
                Name = "Carlos",
                State = "RJ",
                CPF = "96074759090"
            };

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns("96074759090");
            mockRepository.Setup(repository => repository.Get())
                .Returns(new List<Client> { expectedClient }.AsQueryable());

            var clientDTO = ClientMapper.ToClientDTO(expectedClient);
            var controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);

            // Act
            var unformattedClientCPF = "960.747.590-90";
            var result = controller.GetClient(unformattedClientCPF);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var outputDTO = Assert.IsType<ClientDTO>(okResult.Value);
            Assert.Equal(clientDTO.CPF, outputDTO.CPF);
            Assert.Equal(clientDTO.Name, outputDTO.Name);
            Assert.Equal(clientDTO.State, outputDTO.State);
        }

        [Fact]
        public void GetClient_WithInvalidCpf_ReturnsBadRequest()
        {
            // Arrange
            var mockCPFHandler = new Mock<ICPFHandler>();

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(false);

            var controller = new ClientsController(mockCPFHandler.Object, null);

            // Act
            var unformattedClientCPF = "960.747.590-91";
            var result = controller.GetClient(unformattedClientCPF);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid CPF.", badRequestResult.Value);
        }

        [Fact]
        public void GetClient_ClientNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockCPFHandler = new Mock<ICPFHandler>();
            var mockRepository = new Mock<IRepository<Client>>();

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns("96074759090");
            mockRepository.Setup(repository => repository.Get()).Returns(new List<Client> { }.AsQueryable());

            var controller = new ClientsController(mockCPFHandler.Object, mockRepository.Object);

            // Act
            var unformattedClientCPF = "960.747.590-90";
            var result = controller.GetClient(unformattedClientCPF);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Client>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Client not found.", notFoundResult.Value);
        }

        [Fact]
        public async void GetAll_ReturnsOkResult()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Client>>();
            var controller = new ClientsController(null, mockRepository.Object);

            var clients = new List<Client>
            {
                new Client {
                    Name = "Carlos",
                    State = "RJ",
                    CPF = "96074759090"
                },
                 new Client {
                    Name = "Lucas",
                    State = "MG",
                    CPF = "46326462029"
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
