using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Commands.SendEmailRecover;
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

    [HttpPatch("sendEmailRecover")]
    public async Task<IActionResult> SendEmailRecover([FromBody] SendEmailRecoverCommand command) {
        try {
            await this._mediator.Send(command);
            return this.NoContent();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPatch("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command) {
        try {
            await this._mediator.Send(command);
            return this.NoContent();
        } catch (UserNotExistException) {
            return this.NotFound();
        } catch (TokenRecoverPasswordExpiredException) {
            return this.StatusCode(410);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }
}