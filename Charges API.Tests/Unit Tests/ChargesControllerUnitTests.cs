using Charges_API.Controllers;
using Charges_API.DTO;
using Domain.Charges.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Unit_Tests.Controllers
{
    public class ChargesControllerUnitTests
    {
        private readonly Mock<ICPFHandler> mockCPFHandler;
        private readonly Mock<IRepository<Charge>> mockRepository;
        private readonly ChargesController controller;
        private readonly string validCPF, validCPFToNumericString, invalidCPF;

        public ChargesControllerUnitTests()
        {
            mockCPFHandler = new Mock<ICPFHandler>();
            mockRepository = new Mock<IRepository<Charge>>();
            controller = new ChargesController(mockCPFHandler.Object, mockRepository.Object);
            validCPF = "960.747.590-90";
            validCPFToNumericString = "96074759090";
            invalidCPF = "960.747.590-91";
        }

        [Fact]
        public void CreateCharge_ValidDTO_ReturnsCreatedResult()
        {
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            var chargeDTO = new ChargeDTO(10, DateTime.Now, validCPF);

            var result = controller.Create(chargeDTO);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void CreateCharge_InvalidCPF_ReturnsBadRequest()
        {
            var chargeDTO = new ChargeDTO(10, DateTime.Now, invalidCPF);
            var result = controller.Create(chargeDTO);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetAll_FilteredByCPF_ReturnsFilteredCharges()
        {
            var charges = new List<Charge>
            {
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 10 },
                new Charge { ClientCPF = validCPFToNumericString, DueDate = DateTime.Now, Value = 20 },
                new Charge { ClientCPF = validCPFToNumericString, DueDate = DateTime.Now, Value = 25 },
            };

            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns(validCPFToNumericString);
            mockRepository.Setup(repo => repo.Get()).Returns(charges.AsQueryable());

            var result = controller.GetAll(validCPF, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var chargesQueryable = Assert.IsAssignableFrom<IQueryable<ChargeDTO>>(okResult.Value);
            Assert.Equal(2, chargesQueryable.Count());
        }

        [Fact]
        public void GetAll_FilteredByMonth_ReturnsFilteredCharges()
        {
            var charges = new List<Charge>
            {
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 10 },
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 11 },
                new Charge { ClientCPF = validCPFToNumericString, DueDate = DateTime.Now.AddMonths(1), Value = 30 },
            };

            var month = DateTime.Now.AddMonths(1).Month;
            mockRepository.Setup(repo => repo.Get()).Returns(charges.AsQueryable());

            var result = controller.GetAll(null, month);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var chargesQueryable = Assert.IsAssignableFrom<IQueryable<ChargeDTO>>(okResult.Value);
            Assert.Equal(1, chargesQueryable.Count());
        }

        [Fact]
        public void GetAll_FilteredByCPFAndMonth_ReturnsFilteredCharges()
        {
            var charges = new List<Charge>
            {
                new Charge { ClientCPF = validCPFToNumericString, DueDate = DateTime.Now.AddMonths(1), Value = 30 },
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 30 },
                new Charge { ClientCPF = "12345678901", DueDate = DateTime.Now, Value = 20 },
            };

            var month = DateTime.Now.AddMonths(1).Month;
            mockCPFHandler.Setup(handler => handler.IsCpf(It.IsAny<string>())).Returns(true);
            mockCPFHandler.Setup(handler => handler.CPFToNumericString(It.IsAny<string>())).Returns(validCPFToNumericString);
            mockRepository.Setup(repo => repo.Get()).Returns(charges.AsQueryable());

            var result = controller.GetAll(validCPF, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var chargesQueryable = Assert.IsAssignableFrom<IQueryable<ChargeDTO>>(okResult.Value);
            Assert.Equal(1, chargesQueryable.Count());
        }

        [Fact]
        public void GetAll_NoParameters_ReturnsBadRequest()
        {
            var result = controller.GetAll(null, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Atleast one parameter is necessary. Please specify cpf or month.", badRequestResult.Value);
        }

        [Fact]
        public void GetAll_InvalidCPF_ReturnsBadRequest()
        {
            var result = controller.GetAll(invalidCPF, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid CPF.", badRequestResult.Value);
        }

    }
}
