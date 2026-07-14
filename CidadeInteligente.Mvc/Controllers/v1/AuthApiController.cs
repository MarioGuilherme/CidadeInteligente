using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Application.Queries.AuthenticateUser;
using CidadeInteligente.Mvc.Requests.v1;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/auth")]
[ApiController]
public class AuthApiController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        AuthenticateUserQuery authenticateUserQuery = new(request.Email, request.Password);
        AuthenticateUserQueryResult? authenticateUserQueryResult = await _mediator.Send(authenticateUserQuery);

        if (authenticateUserQueryResult is not null)
        {
            ClaimsIdentity identity = new(authenticateUserQueryResult.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new() { IsPersistent = true });
        }

        return Created();
    }

    [HttpPatch("send-email-recover")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> SendEmailRecover([FromBody] SendEmailRecoverRequest request)
    {
        SendEmailRecoverCommand sendEmailRecoverCommand = new(request.Email, $"{Request.Scheme}://{Request.Host}");
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
