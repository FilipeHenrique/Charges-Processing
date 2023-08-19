using Microsoft.Extensions.Logging;
using Quartz;

namespace Charges_Processing_Job
{
    public class ChargesProcessingJob : IJob
    {
        private readonly ILogger<ChargesProcessingJob> _logger;

        public ChargesProcessingJob(ILogger<ChargesProcessingJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("RODOU");
            return Task.CompletedTask;
        }
    }
}