using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>();
            return services;
        }
    }
}
