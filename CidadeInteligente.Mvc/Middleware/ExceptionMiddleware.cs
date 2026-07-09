using CidadeInteligente.Mvc.Responses;

namespace CidadeInteligente.Mvc.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new RestResponse
            {
                Notifications = ["Internal error"]
            });

            logger.LogError(ex, "Internal error in the CidadeInteligente App");
        }
    }
}
