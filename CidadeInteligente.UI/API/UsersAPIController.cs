using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CidadeInteligente.UI.API;

[Route("API/auth")]
public class UsersAPIController(ILogger<UsersAPIController> logger, IMediator mediator) : ControllerBase {
    private readonly ILogger<UsersAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command) {
        User? user = await this._mediator.Send(command);

        if (user is null) return this.NotFound();

        ClaimsIdentity claimsIdentity = new(
            [new(nameof(user.UserId), user.UserId.ToString()), new(ClaimTypes.Role, user.Role.ToString())],
            "CookieAuth"
        );

        AuthenticationProperties authProperties = new() {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await this.HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

        return this.Created();
    }
}