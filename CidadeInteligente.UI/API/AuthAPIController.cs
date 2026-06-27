using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CidadeInteligente.UI.API;

[Route("API/auth")]
public class AuthAPIController(ILogger<AuthAPIController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<AuthAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        LoginViewModel user = await _mediator.Send(command);
        ClaimsIdentity claimsIdentity = new([
            new(nameof(user.UserId), user.UserId.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
        ], "Cookie");
        AuthenticationProperties authProperties = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(claimsIdentity), authProperties);
        return Created();
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