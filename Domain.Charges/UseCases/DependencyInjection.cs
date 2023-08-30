using Domain.Charges.Interfaces.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Charges.UseCases
{
    public static class DependencyInjection
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddTransient<ICreateChargeUseCase, CreateChargeUseCase>();
            services.AddTransient<IGetChargesUseCase, GetChargesUseCase>();
        }
    }
}
