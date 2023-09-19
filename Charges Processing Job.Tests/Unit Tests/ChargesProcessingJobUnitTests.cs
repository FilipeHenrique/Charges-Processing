using Charges_Processing_Job;
using Domain.Charges.Entities;
using Domain.Clients.Entities;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.Unit_Tests.Charges_Processing_Job
{
    public class ChargesProcessingJobUnitTests
    {
        private readonly Mock<HttpMessageHandler> httpClientHandlerMock = new Mock<HttpMessageHandler>();
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private const string validCPF = "960.747.590-90";
        private const string validCPF2 = "183.610.120-10";
        private const string validURI = "http://localhost:3000";

        [Fact]
        public async Task GetClients_Should_Return_Clients()
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

            httpClientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(clients), Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpClientHandlerMock.Object)
            {
                BaseAddress = new Uri(validURI)
            };

            httpClientFactoryMock.Setup(factory =>
                factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var job = new ChargesProcessingJob(httpClientFactoryMock.Object);

            var clientsResponse = await job.GetClients().ToListAsync();
            Assert.Equal(2,clientsResponse.Count);
        }

        [Fact]
        public async Task CreateCharges_Should_Create_Charges_For_Clients()
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

            }.ToAsyncEnumerable();

            var charges = new List<Charge>
            {
                 new Charge
                 {
                     ClientCPF = validCPF,
                     DueDate = DateTime.Now,
                     Value = 9690
                 },

                 new Charge
                 {
                     ClientCPF = validCPF2,
                     DueDate = DateTime.Now,
                     Value = 1810
                 }

            };

            httpClientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(charges), Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpClientHandlerMock.Object)
            {
                BaseAddress = new Uri(validURI)
            };

            httpClientFactoryMock.Setup(factory =>
                factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var job = new ChargesProcessingJob(httpClientFactoryMock.Object);

            var chargesResult = await job.CreateCharges(clients).ToListAsync();
            Assert.Equal(2, chargesResult.Count);
            Assert.Equal(9690, chargesResult[0].value);
        }

        [Fact]
        public async Task Report_Should_Write_Report_File()
        {
            var chargesByState = new List<(string state, float value)>
            {
                ("MG", 100.0f),
                ("RJ", 200.0f)
            }.ToAsyncEnumerable();

            var job = new ChargesProcessingJob(httpClientFactoryMock.Object);

            await job.Report(chargesByState);

            var currentDirectory = Directory.GetCurrentDirectory();
            var desiredDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..")); // removes \bin\Debug\net6.0
            var filePath = Path.Combine(desiredDirectory, "Report.txt");

            var fileContents = await File.ReadAllLinesAsync(filePath);
            Assert.Equal("State: MG, Total: 100", fileContents[0]);
            Assert.Equal("State: RJ, Total: 200", fileContents[1]);

            File.Delete(filePath);
        }
    }
}
