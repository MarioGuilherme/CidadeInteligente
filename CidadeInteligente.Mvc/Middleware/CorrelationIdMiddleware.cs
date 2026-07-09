using Serilog.Context;

namespace CidadeInteligente.Mvc.Middleware;

public partial class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        string correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Response.Headers["X-Correlation-ID"] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            LogStartOfRequest(correlationId);
            await _next(context);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Start of request with CorrelationId {CorrelationId}.")]
    private partial void LogStartOfRequest(string correlationId);
}
