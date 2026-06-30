using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.Responses;
using CidadeInteligente.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;

namespace CidadeInteligente.Mvc.Filters;

//public class RestResponseFilter(INotificationContext notification) : IAsyncResultFilter
//{
//    private readonly INotificationContext _notification = notification;

//    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
//    {
//        if (context.Result is ViewResult)
//        {
//            if (_notification.HasNotifications)
//            {
//                IModelMetadataProvider metadataProvider = context.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();
//                ViewDataDictionary<ErrorViewModel> viewData = new(metadataProvider, context.ModelState)
//                {
//                    Model = new ErrorViewModel(MapStatusCode(_notification.Notifications), string.Join(", ", _notification.Notifications))
//                };

//                context.Result = new ViewResult
//                {
//                    ViewName = "/Views/Error.cshtml",
//                    ViewData = viewData
//                };

//                await next();
//                return;
//            }

//            await next();
//            return;
//        }

//        if (_notification.HasValidations)
//        {
//            context.Result = new BadRequestObjectResult(new RestResponseWithInvalidFields { InvalidFields = _notification.Validations });
//            await next();
//            return;
//        }

//        if (_notification.HasNotifications && (context.Result is NoContentResult || context.Result is AcceptedResult))
//        {
//            context.Result = new ObjectResult(new RestResponse { Notifications = _notification.AsListString })
//            {
//                StatusCode = MapStatusCode(_notification.Notifications)
//            };
//            await next();
//            return;
//        }

//        if (context.Result is ObjectResult objectResult && objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
//        {
//            if (objectResult.Value is not null)
//            {
//                RestResponse restResponse = new(objectResult.Value);

//                if (_notification.HasNotifications)
//                {
//                    objectResult.StatusCode = (int)HttpStatusCode.MultiStatus;
//                    objectResult.Value = restResponse with { Notifications = _notification.AsListString };
//                }
//                else
//                    objectResult.Value = restResponse;
//            }
//            else if (objectResult is not CreatedResult)
//            {
//                context.Result = new ObjectResult(new RestResponse { Notifications = _notification.AsListString })
//                {
//                    StatusCode = MapStatusCode(_notification.Notifications)
//                };
//            }
//        }

//        await next();
//    }

//    private static int MapStatusCode(IReadOnlyCollection<Notification> notifications)
//    {
//        if (notifications.Any(n => n.Type == NotificationType.UserNotFound
//            || n.Type == NotificationType.AreaNotFound
//            || n.Type == NotificationType.ProjectNotFound
//            || n.Type == NotificationType.CourseNotFound
//            || n.Type == NotificationType.UserWithTokenNotFound
//            || n.Type == NotificationType.UserWithEmailNotFound))
//            return StatusCodes.Status404NotFound;

//        if (notifications.Any(n => n.Type == NotificationType.UserNotAuthorizedToModifyProject))
//            return StatusCodes.Status403Forbidden;

//        if (notifications.Any(n => n.Type == NotificationType.InvalidLoginCredentials
//            || n.Type == NotificationType.TokenRecoverPasswordExpired))
//            return StatusCodes.Status401Unauthorized;

//        if (notifications.Any(n => n.Type == NotificationType.EmailAlreadyInUse
//            || n.Type == NotificationType.CourseWithDependentProjects
//            || n.Type == NotificationType.AreaWithDependentProjects
//            || n.Type == NotificationType.UserWithDependentProjects))
//            return StatusCodes.Status409Conflict;

//        return StatusCodes.Status400BadRequest;
//    }
//}

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
        // MVC: troca para a página de erro quando há notifications
        if (context.Result is ViewResult)
            return _notification.HasNotifications ? BuildErrorView(context) : context.Result;

        // API: falhas de validação => 400 com os campos inválidos
        if (_notification.HasValidations)
            return new BadRequestObjectResult(new RestResponseWithInvalidFields { InvalidFields = _notification.Validations });

        // API: notifications em resultados sem corpo => status mapeado
        if (_notification.HasNotifications && context.Result is NoContentResult or AcceptedResult)
            return BuildNotificationResult();

        // API: envelopa ObjectResults de sucesso
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

        // Value null: CreatedResult passa intacto; o resto vira resposta de notification
        return objectResult is CreatedResult ? objectResult : BuildNotificationResult();
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
            NotificationType.CourseWithDependentProjects,
            NotificationType.AreaWithDependentProjects,
            NotificationType.UserWithDependentProjects
        ]),
    ];
}