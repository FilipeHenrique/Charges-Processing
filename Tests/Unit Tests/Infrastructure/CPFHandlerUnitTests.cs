using Infrastructure.Services;

namespace Tests.Unit_Tests.Services
{

    public class CPFHandlerUnitTests
    {
        private CPFHandler cpfHandler;
        private readonly string validCPF, validCPFToNumericString, invalidCPF;

        public CPFHandlerUnitTests()
        {
            cpfHandler = new CPFHandler();
            validCPF = "960.747.590-90";
            validCPFToNumericString = "96074759090";
            invalidCPF = "960.747.590-91";
        }

        [Fact]
        public void IsCpf_ValidCpf_ReturnsTrue()
        {
            // Act
            bool result = cpfHandler.IsCpf(validCPF);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsCpf_InvalidCpf_ReturnsFalse()
        {
            // Act
            bool result = cpfHandler.IsCpf(invalidCPF);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CPFToNumericString_ValidCpf_ReturnsNumericString()
        {
            // Act
            string result = cpfHandler.CPFToNumericString(validCPF);

            // Assert
            Assert.Equal(validCPFToNumericString, result);
        }
    }
}
