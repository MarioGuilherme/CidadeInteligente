using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CidadeInteligente.UI.API;

[Route("API/auth")]
public class AuthAPIController(ILogger<AuthAPIController> logger, IMediator mediator) : ControllerBase {
    private readonly ILogger<AuthAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command) {
        try {
            LoginViewModel user = await this._mediator.Send(command);
            ClaimsIdentity claimsIdentity = new([
                new(nameof(user.UserId), user.UserId.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
            ], "Cookie");
            AuthenticationProperties authProperties = new() {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await this.HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(claimsIdentity), authProperties);
            return this.Created();
        } catch (EmailOrPasswordNotMatchException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }
}