using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Application.Queries.AuthenticateUser;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.Requests.v1;
using CidadeInteligente.Mvc.Responses;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/auth")]
[ApiController]
public class AuthApiController(INotificationContext notification, IMediator mediator) : ControllerBase
{
    private readonly INotificationContext _notification = notification;
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        AuthenticateUserQuery authenticateUserQuery = new(request.Email, request.Password);
        AuthenticateUserQueryResult? authenticateUserQueryResult = await _mediator.Send(authenticateUserQuery);

        if (authenticateUserQueryResult is not null)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticateUserQueryResult.ClaimsPrincipal, new() { IsPersistent = true });

        return Created();
    }

    [HttpPatch("send-email-recover")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> SendEmailRecover([FromBody] SendEmailRecoverRequest request)
    {
        SendEmailRecoverCommand sendEmailRecoverCommand = new(request.Email);
        await _mediator.Send(sendEmailRecoverCommand);
        return NoContent();
    }

    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        ChangePasswordCommand changePasswordCommand = new(request.NewPassword, request.ConfirmNewPassword, request.Token);
        await _mediator.Send(changePasswordCommand);
        return NoContent();
    }
}
