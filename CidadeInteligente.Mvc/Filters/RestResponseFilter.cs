using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CidadeInteligente.Mvc.Filters;

public class RestResponseFilter(INotificationContext notification) : IAsyncResultFilter
{
    private readonly INotificationContext _notification = notification;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (_notification.HasValidations)
        {
            context.Result = new BadRequestObjectResult(new RestResponseWithInvalidFields { InvalidFields = _notification.Validations });
            await next();
            return;
        }

        if (context.Result is NoContentResult || context.Result is AcceptedResult)
        {
            if (!_notification.HasNotifications)
            {
                await next();
                return;
            }

            context.Result = new ObjectResult(new RestResponse { Notifications = _notification.AsListString })
            {
                StatusCode = MapStatusCode(_notification.Notifications)
            };
            await next();
            return;
        }

        if (context.Result is ObjectResult objectResult && objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
        {
            if (objectResult.Value is not null)
            {
                RestResponse restResponse = new(objectResult.Value);

                if (_notification.HasNotifications)
                {
                    objectResult.StatusCode = (int)HttpStatusCode.MultiStatus;
                    objectResult.Value = restResponse with { Notifications = _notification.AsListString };
                }
                else
                    objectResult.Value = restResponse;
            }
            else
            {
                context.Result = new ObjectResult(new RestResponse { Notifications = _notification.AsListString })
                {
                    StatusCode = MapStatusCode(_notification.Notifications)
                };
            }
        }

        await next();
    }

    private static int MapStatusCode(IReadOnlyCollection<Notification> notifications)
    {
        if (notifications.Any(n => n.Type == NotificationType.UserNotFound || n.Type == NotificationType.AreaNotFound || n.Type == NotificationType.ProjectNotFound
            || n.Type == NotificationType.CourseNotFound || n.Type == NotificationType.UserWithTokenNotFound || n.Type == NotificationType.UserWithEmailNotFound))
            return StatusCodes.Status404NotFound;

        if (notifications.Any(n => n.Type == NotificationType.UserNotAuthorizedToModifyProject || n.Type == NotificationType.AreaWithDependentProjects)
            || notifications.Any(n => n.Type == NotificationType.CourseWithDependentProjects || n.Type == NotificationType.UserWithDependentProjects))
            return StatusCodes.Status403Forbidden;

        if (notifications.Any(n => n.Type == NotificationType.InvalidLoginCredentials || n.Type == NotificationType.TokenRecoverPasswordExpired))
            return StatusCodes.Status401Unauthorized;

        if (notifications.Any(n => n.Type == NotificationType.EmailAlreadyInUse))
            return StatusCodes.Status409Conflict;

        return StatusCodes.Status400BadRequest;
    }
}
