using System.Net;
using System.Text.Json;

namespace GymApp.Configuration.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "A wild exception appears");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = context.Response.StatusCode,
                message = "Erro inesperado. Consulte os logs para mais detalhes."
            };

            var payload = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(payload);
        }
    }
}
