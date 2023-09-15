using Charges_Processing_Job;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.Integration_Tests.Charges_Processing_Job
{
    public class ChargesProcessingJobIntegrationTests
    {
        private readonly WebApplicationFactory<Program> factory;

        public ChargesProcessingJobIntegrationTests()
        {
            factory = new WebApplicationFactory<Program>();
        }

        [Fact]
        public async Task Execute_ChargesProcessingJob_Success()
        {
            // Arrange
            var httpClient = factory.CreateClient();
            var apiUrlsConfig = new ApiUrlsConfig();

            var job = new ChargesProcessingJob(httpClient, apiUrlsConfig);

            // Act
            await job.Execute(null); // You can pass a mock IJobExecutionContext if needed.

            // Assert
            // Add assertions here to verify the expected behavior of your job.
        }
    }
}
