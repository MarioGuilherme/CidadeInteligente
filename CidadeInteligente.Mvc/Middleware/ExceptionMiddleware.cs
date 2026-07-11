using CidadeInteligente.Mvc.Responses;

namespace CidadeInteligente.Mvc.Middleware;

public partial class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            LogRequestCancelled(context.Request.Path);
        }
        catch (Exception ex)
        {
            LogUnhandledException(ex, context.Request.Path);

            if (context.Response.HasStarted) return;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new RestResponse
            {
                Notifications = ["Erro interno"]
            });
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Request cancelled by the client on {Path}.")]
    private partial void LogRequestCancelled(string path);

    [LoggerMessage(Level = LogLevel.Error, Message = "Internal error in the CidadeInteligente App on {Path}.")]
    private partial void LogUnhandledException(Exception ex, string path);
}
