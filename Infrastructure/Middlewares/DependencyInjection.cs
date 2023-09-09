using Microsoft.AspNetCore.Builder;
namespace Infrastructure.Middlewares
{
    public static class DependencyInjection
    {
        public static IApplicationBuilder AddErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
