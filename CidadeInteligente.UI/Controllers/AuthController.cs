using CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.UI.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

public class AuthController(ILogger<AuthController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet("login")]
    public IActionResult Login() => !(User.Identity?.IsAuthenticated ?? false) ? View() : Redirect("/");

    [HttpGet("recuperar-senha")]
    public IActionResult RecuperarSenha() => !(User.Identity?.IsAuthenticated ?? false) ? View() : Redirect("/");

    [HttpGet("alterar-senha")]
    public async Task<IActionResult> AlterarSenha(string token)
    {
        try
        {
            if (User.Identity?.IsAuthenticated ?? false)
                return Redirect("/");
            GetUserByTokenRecoverPasswordQuery getUserByTokenRecoverPasswordQuery = new(token);
            GetUserByTokenRecoverPasswordQueryResult getUserByTokenRecoverPasswordQueryResult = await _mediator.Send(getUserByTokenRecoverPasswordQuery);
            return View(getUserByTokenRecoverPasswordQueryResult);
        }
        catch (Exception ex) when (ex is UserNotExistException || ex is TokenRecoverPasswordExpiredException || ex is ValidationException)
        {
            return Redirect("/");
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [HttpGet("sem-permissao")]
    [Authorize]
    public ViewResult SemPermissao() => View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não tem permissão para acessar está página!"));
}