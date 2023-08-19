using Quartz;

namespace Charges_Processing_Job
{
    public class ChargesProcessingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Job executed at: " + DateTime.Now);
            return Task.CompletedTask;
        }
    }
}