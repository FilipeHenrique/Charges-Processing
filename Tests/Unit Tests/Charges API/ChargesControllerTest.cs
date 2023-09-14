using Charges_API.Controllers;
using Charges_API.DTO;
using Domain.Charges.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Unit_Tests.Controllers
{
    public class ChargesControllerTest
    {
        private readonly Mock<ICPFHandler> mockCPFHandler;
        private readonly Mock<IRepository<Charge>> mockRepository;
        private readonly ChargesController controller;

        public ChargesControllerTest()
        {
            mockCPFHandler = new Mock<ICPFHandler>();
            mockRepository = new Mock<IRepository<Charge>>();
            controller = new ChargesController(mockCPFHandler.Object, mockRepository.Object);
        }

        [Fact]
        public void CreateCharge_ValidDTO_ReturnsCreatedResult()
        {
            // Arrange
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            var chargeDTO = new ChargeDTO(10, DateTime.Now, "960.747.590-90");

            // Act
            var result = controller.Create(chargeDTO);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void CreateCharge_InvalidCPF_ReturnsBadRequest()
        {
            // Arrange
            var chargeDTO = new ChargeDTO(10, DateTime.Now, "960.747.590-91");

            // Act
            var result = controller.Create(chargeDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetAll_FilteredByCPF_ReturnsFilteredCharges()
        {
            // Arrange
            var charges = new List<Charge>
            {
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 10 },
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 11 },
                new Charge { ClientCPF = "98765432109", DueDate = DateTime.Now, Value = 30 },
                new Charge { ClientCPF = "96074759090", DueDate = DateTime.Now, Value = 20 },
                new Charge { ClientCPF = "96074759090", DueDate = DateTime.Now, Value = 25 },
            };

            var clientCPF = "960.747.590-90";
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns("96074759090");
            mockRepository.Setup(repo => repo.Get()).Returns(charges.AsQueryable());

            // Act
            var result = controller.GetAll(clientCPF, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var chargesQueryable = Assert.IsAssignableFrom<IQueryable<ChargeDTO>>(okResult.Value);
            Assert.Equal(2, chargesQueryable.Count());
        }

        [Fact]
        public void GetAll_FilteredByMonth_ReturnsFilteredCharges()
        {
            // Arrange
            var charges = new List<Charge>
            {
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 10 },
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 11 },
                new Charge { ClientCPF = "98765432109", DueDate = DateTime.Now.AddMonths(1), Value = 30 },
            };

            var month = DateTime.Now.AddMonths(1).Month;
            mockRepository.Setup(repo => repo.Get()).Returns(charges.AsQueryable());

            // Act
            var result = controller.GetAll(null, month);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var chargesQueryable = Assert.IsAssignableFrom<IQueryable<ChargeDTO>>(okResult.Value);
            Assert.Equal(1, chargesQueryable.Count());
        }

        [Fact]
        public void GetAll_FilteredByCPFAndMonth_ReturnsFilteredCharges()
        {
            // Arrange
            var charges = new List<Charge>
            {
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 10 },
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 11 },
                new Charge { ClientCPF = "98765432109", DueDate = DateTime.Now, Value = 30 },
                new Charge { ClientCPF = "96074759090", DueDate = DateTime.Now, Value = 20 },
                new Charge { ClientCPF = "98765432109", DueDate = DateTime.Now.AddMonths(1), Value = 30 },
            };

            var clientCPF = "960.747.590-90";
            var month = DateTime.Now.AddMonths(1).Month;
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns("96074759090");
            mockRepository.Setup(repo => repo.Get()).Returns(charges.AsQueryable());

            // Act
            var result = controller.GetAll(clientCPF, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var chargesQueryable = Assert.IsAssignableFrom<IQueryable<ChargeDTO>>(okResult.Value);
            Assert.Equal(1, chargesQueryable.Count());
        }

        [Fact]
        public void GetAll_NoParameters_ReturnsBadRequest()
        {
            // Act
            var result = controller.GetAll(null, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Atleast one parameter is necessary. Please specify cpf or month.", badRequestResult.Value);
        }

        [Fact]
        public void GetAll_InvalidCPF_ReturnsBadRequest()
        {
            // Act
            var invalidCPF = "960.747.590-91";
            var result = controller.GetAll(invalidCPF, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid CPF.", badRequestResult.Value);
        }

    }
}
