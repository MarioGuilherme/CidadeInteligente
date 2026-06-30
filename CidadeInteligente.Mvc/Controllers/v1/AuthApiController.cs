using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Mvc.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/auth")]
public class AuthApiController(INotificationContext notification, IMediator mediator) : ControllerBase
{
    private readonly INotificationContext notification = notification;
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        LoginUserCommand loginUserCommand = new(request.Email, request.Password);
        LoginUserCommandResult? loginUserCommandResult = await _mediator.Send(loginUserCommand);

        if (loginUserCommandResult is not null)
        {
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                loginUserCommandResult!.ClaimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });

            return Created();
        }

        return default;
    }

    [HttpPatch("sendEmailRecover")]
    public async Task<IActionResult> SendEmailRecover([FromBody] SendEmailRecoverCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
