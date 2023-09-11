using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Charges_Processing_Job
{
    public static class DependencyInjection
    {
        public static void AddJob(this IServiceCollection services)
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
                                schedule.WithIntervalInSeconds(10).RepeatForever()
                            )
                        );
            });
            services.AddQuartzHostedService();
        }
    }
}
