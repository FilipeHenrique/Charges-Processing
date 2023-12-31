﻿using Infrastructure.Services;

namespace Infrastructure.Tests
{

    public class CPFHandlerUnitTests
    {
        private CPFHandler cpfHandler = new CPFHandler();
        private const string validCPF = "960.747.590-90";
        private const string validCPFToNumericString = "96074759090";
        private const string invalidCPF = "960.747.590-91";

        [Fact]
        public void IsCpf_ValidCpf_ReturnsTrue()
        {
            bool result = cpfHandler.IsCpf(validCPF);
            Assert.True(result);
        }

        [Fact]
        public void IsCpf_InvalidCpf_ReturnsFalse()
        {
            bool result = cpfHandler.IsCpf(invalidCPF);
            Assert.False(result);
        }

        [Fact]
        public void CPFToNumericString_ValidCpf_ReturnsNumericString()
        {
            string result = cpfHandler.CPFToNumericString(validCPF);
            Assert.Equal(validCPFToNumericString, result);
        }
    }
}
