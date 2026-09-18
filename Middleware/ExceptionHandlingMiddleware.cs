using System.Net;
using System.Text.Json;

namespace StudentManagementApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            if (ex is InvalidOperationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                context.Response.ContentType = "application/json";

                var conflictResponse = new
                {
                    statusCode = 409,
                    message = ex.Message
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(conflictResponse));

                return;
            }

            _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                statusCode = 500,
                message = "Unexpected Error Occured"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

}