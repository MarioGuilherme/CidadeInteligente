using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.Responses;
using CidadeInteligente.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;

namespace CidadeInteligente.Mvc.Filters;

public class NotificationResultFilter(INotificationContext notification) : IAsyncResultFilter
{
    private readonly INotificationContext _notification = notification;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        context.Result = TransformResult(context);
        await next();
    }

    private IActionResult TransformResult(ResultExecutingContext context)
    {
        if (context.Result is ViewResult)
            return _notification.HasNotifications ? BuildErrorView(context) : context.Result;

        if (_notification.HasValidations)
            return new BadRequestObjectResult(new RestResponse<IReadOnlyDictionary<string, string[]>> { Notifications = _notification.Validations });

        if (_notification.HasNotifications)
            return BuildNotificationResult();

        if (context.Result is ObjectResult { StatusCode: >= 200 and < 300 } objectResult)
            return WrapSuccessfulResult(objectResult);

        return context.Result;
    }

    private ViewResult BuildErrorView(ResultExecutingContext context)
    {
        IModelMetadataProvider metadataProvider = context.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();
        ViewDataDictionary<ErrorViewModel> viewData = new(metadataProvider, context.ModelState)
        {
            Model = new ErrorViewModel(MapStatusCode(_notification.Notifications), string.Join(", ", _notification.AsListString))
        };

        return new ViewResult
        {
            ViewName = "/Views/Error.cshtml",
            ViewData = viewData
        };
    }

    private ObjectResult BuildNotificationResult() => new(new RestResponse { Notifications = _notification.AsListString })
    {
        StatusCode = MapStatusCode(_notification.Notifications)
    };

    private ObjectResult WrapSuccessfulResult(ObjectResult objectResult)
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
            {
                objectResult.Value = restResponse;
            }

            return objectResult;
        }

        return objectResult is CreatedResult
            || objectResult is CreatedAtRouteResult
            || objectResult is CreatedAtActionResult ? objectResult : BuildNotificationResult();
    }

    private static int MapStatusCode(IReadOnlyCollection<Notification> notifications)
    {
        foreach ((int status, NotificationType[] types) in StatusMap)
            if (notifications.Any(n => types.Contains(n.Type)))
                return status;

        return StatusCodes.Status400BadRequest;
    }

    private static readonly (int Status, NotificationType[] Types)[] StatusMap =
    [
        (StatusCodes.Status404NotFound, [
            NotificationType.UserNotFound,
            NotificationType.AreaNotFound,
            NotificationType.ProjectNotFound,
            NotificationType.CourseNotFound,
            NotificationType.UserWithTokenNotFound,
            NotificationType.UserWithEmailNotFound
        ]),
        (StatusCodes.Status403Forbidden, [
            NotificationType.UserNotAuthorizedToModifyProject
        ]),
        (StatusCodes.Status401Unauthorized, [
            NotificationType.InvalidLoginCredentials,
            NotificationType.TokenRecoverPasswordExpired
        ]),
        (StatusCodes.Status409Conflict, [
            NotificationType.EmailAlreadyInUse,
            NotificationType.CourseWithDependentProjectsOrUser,
            NotificationType.AreaWithDependentProjects,
            NotificationType.UserWithDependentProjects
        ])
    ];
}
