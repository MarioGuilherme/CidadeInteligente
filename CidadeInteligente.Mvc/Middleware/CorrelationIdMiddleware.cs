using Serilog;
using Serilog.Context;
using Serilog.Core;

namespace CidadeInteligente.Mvc.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<CorrelationIdMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        string correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Response.Headers["X-Correlation-ID"] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogInformation("Start of request with CorrelationId {CorrelationId}.", correlationId);
            await _next(context);
        }
    }
}
