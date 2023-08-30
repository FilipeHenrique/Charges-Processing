using Domain.Clients.Interfaces.UseCases;
using Microsoft.Extensions.DependencyInjection;


namespace Domain.Clients.UseCases
{
    public static class DependencyInjection
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddTransient<ICreateClientUseCase, CreateClientUseCase>();
            services.AddTransient<IGetClientUseCase, GetClientUseCase>();
            services.AddTransient<IGetAllClientsUseCase, GetAllClientsUseCase>();
        }
    }
}
