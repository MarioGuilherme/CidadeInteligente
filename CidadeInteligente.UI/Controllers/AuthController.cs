using CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.UI.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

public class AuthController(ILogger<AuthController> logger, IMediator mediator) : Controller {
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet("login")]
    public IActionResult Login() => !(this.User.Identity?.IsAuthenticated ?? false) ? this.View() : this.Redirect("/");

    [HttpGet("recuperar-senha")]
    public IActionResult RecuperarSenha() => !(this.User.Identity?.IsAuthenticated ?? false) ? this.View() : this.Redirect("/");

    [HttpGet("alterar-senha")]
    public async Task<IActionResult> AlterarSenha(string token) {
        try {
            if (this.User.Identity?.IsAuthenticated ?? false)
                return this.Redirect("/");
            GetUserByTokenRecoverPasswordQuery getUserByTokenRecoverPasswordQuery = new(token);
            UserDataChangePassword userFormChangePassword = await this._mediator.Send(getUserByTokenRecoverPasswordQuery);
            return this.View(userFormChangePassword);
        } catch (Exception ex) when (ex is UserNotExistException || ex is TokenRecoverPasswordExpiredException || ex is ValidationException) {
            return this.Redirect("/");
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<IActionResult> Logout() {
        await this.HttpContext.SignOutAsync();
        return this.Redirect("/");
    }

    [HttpGet("sem-permissao")]
    [Authorize]
    public ViewResult SemPermissao() => this.View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não tem permissão para acessar está página!"));
}