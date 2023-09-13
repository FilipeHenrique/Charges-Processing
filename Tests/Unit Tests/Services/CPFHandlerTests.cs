using Infrastructure.Services;

namespace Tests.Unit_Tests.Services
{

    public class CPFHandlerTests
    {
        private CPFHandler cpfHandler;

        public CPFHandlerTests()
        {
            cpfHandler = new CPFHandler();
        }

        [Fact]
        public void IsCpf_ValidCpf_ReturnsTrue()
        {
            // Arrange
            string validCpf = "463.264.620-29";

            // Act
            bool result = cpfHandler.IsCpf(validCpf);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsCpf_InvalidCpf_ReturnsFalse()
        {
            // Arrange
            string invalidCpf = "463.264.620-21";

            // Act
            bool result = cpfHandler.IsCpf(invalidCpf);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CPFToNumericString_ValidCpf_ReturnsNumericString()
        {
            // Arrange
            string cpf = "463.264.620-29";
            string expectedFormattedCPF = "46326462029";

            // Act
            string result = cpfHandler.CPFToNumericString(cpf);

            // Assert
            Assert.Equal(expectedFormattedCPF, result);
        }
    }
}
