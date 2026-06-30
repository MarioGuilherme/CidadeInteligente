using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Mvc.Requests.v1;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/auth")]
public class AuthApiController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        LoginUserCommand loginUserCommand = new(request.Email, request.Password);
        LoginUserCommandResult? loginUserCommandResult = await _mediator.Send(loginUserCommand);

        if (loginUserCommandResult is null)
        {
            return default;
        }

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

    [HttpPatch("send-email-recover")]
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
