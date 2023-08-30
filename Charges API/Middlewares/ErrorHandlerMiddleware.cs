using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Charges_API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;


        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Title = "Server Error",
                    Type = "Error",
                    Detail = error.Message,
                    Status = (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        }
    }
}
