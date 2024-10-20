using CidadeInteligente.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CidadeInteligente.UI.ExceptionHandler;

public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler {
    private readonly ILogger<ApiExceptionHandler> _logger = logger;

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
        if (!httpContext.Request.Path.ToString().StartsWith("/API")) // As chamadas REST sempre começará a URI com "/API"
            return ValueTask.FromResult(false);

        httpContext.Response.StatusCode = exception switch {
            EmailOrPasswordNotMatchException or
            UserNotExistException or
            CourseNotExistException or
            AreaNotExistException or
            ProjectNotExistException => StatusCodes.Status404NotFound,

            SendEmailException => StatusCodes.Status424FailedDependency,

            TokenRecoverPasswordExpiredException => StatusCodes.Status410Gone,

            EmailAlreadyInUseException or
            UserWithDepedentProjectsException or
            CourseWithDepedentProjectsException or
            AreaWithDepedentProjectsException => StatusCodes.Status409Conflict,

            UserIsReadOnlyException => StatusCodes.Status403Forbidden,

            _ => StatusCodes.Status500InternalServerError,
        };

        this._logger.LogError("{Message}", exception.Message);

        return ValueTask.FromResult(true);
    }
}