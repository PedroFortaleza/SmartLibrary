using System.Text.Json;
using SmartLibrary.Application.Common;

namespace SmartLibrary.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BusinessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            context.Response.ContentType = "application/json";
            var body = JsonSerializer.Serialize(new { message = ex.Message });
            await context.Response.WriteAsync(body);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            var body = JsonSerializer.Serialize(new { message = ex.Message });
            await context.Response.WriteAsync(body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro interno não tratado.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var body = JsonSerializer.Serialize(new { message = "Erro interno do servidor." });
            await context.Response.WriteAsync(body);
        }
    }
}
