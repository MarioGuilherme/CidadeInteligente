using CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Mvc.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CidadeInteligente.Mvc.Controllers;

public class AuthController(INotificationContext notification, IMediator mediator) : Controller
{
    private readonly INotificationContext notification = notification;
    private readonly IMediator _mediator = mediator;

    [HttpGet("login")]
    public ActionResult Login() => !(User.Identity?.IsAuthenticated ?? false) ? View() : Redirect("/");

    [HttpGet("recuperar-senha")]
    public ActionResult RecuperarSenha() => !(User.Identity?.IsAuthenticated ?? false) ? View() : Redirect("/");

    [HttpGet("alterar-senha")]
    public async Task<ActionResult> AlterarSenha(string token)
    {
        try
        {
            if (User.Identity?.IsAuthenticated ?? false) return Redirect("/");

            GetUserByTokenRecoverPasswordQuery getUserByTokenRecoverPasswordQuery = new(token);
            GetUserByTokenRecoverPasswordQueryResult? getUserByTokenRecoverPasswordQueryResult = await _mediator.Send(getUserByTokenRecoverPasswordQuery);
            return View(getUserByTokenRecoverPasswordQueryResult);
        }
        //catch (Exception ex) when (ex is UserNotExistException || ex is TokenRecoverPasswordExpiredException || ex is ValidationException)
        //{
        //    return Redirect("/");
        //}
        catch (Exception ex)
        {
            Log.Error(ex.Message, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<RedirectResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [HttpGet("sem-permissao")]
    [Authorize]
    public ViewResult SemPermissao() => View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não tem permissão para acessar está página!"));
}
