using Charges_Processing_Job;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.Services.Charges_Processing_Job
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
            var jobKey = JobKey.Create(nameof(ChargesProcessingJob));
            options
                .AddJob<ChargesProcessingJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(5).RepeatForever()
                        )
                    );
            });
            services.AddQuartzHostedService();
        }
    }
}
