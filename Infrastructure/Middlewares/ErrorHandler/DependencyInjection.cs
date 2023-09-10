using Microsoft.AspNetCore.Builder;
namespace Infrastructure.Middlewares.ErrorHandler
{
    public static class DependencyInjection
    {
        public static IApplicationBuilder AddErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
