using CidadeInteligente.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CidadeInteligente.UI.ExceptionHandler;

public class ApiExceptionHandler : IExceptionHandler {
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
        if (!httpContext.Request.Path.ToString().StartsWith("/API")) // As chamadas REST sempre começará a URI com "/API"
            return ValueTask.FromResult(true);

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

        Console.WriteLine(exception.Message);

        return ValueTask.FromResult(true);
    }
}