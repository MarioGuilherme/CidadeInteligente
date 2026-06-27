using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Mvc.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CidadeInteligente.Mvc.API;

[Route("API/auth")]
public class AuthAPIController(ILogger<AuthAPIController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<AuthAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        LoginUserCommand loginUserCommand = new(request.Email, request.Password);
        LoginUserCommandResult loginUserCommandResult = await _mediator.Send(loginUserCommand);
        ClaimsIdentity claimsIdentity = new([
            new(nameof(loginUserCommandResult.UserId), loginUserCommandResult.UserId.ToString()),
            new(ClaimTypes.Role, loginUserCommandResult.Role.ToString())
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