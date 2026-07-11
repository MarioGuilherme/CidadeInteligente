using CidadeInteligente.Application.Commands.GetOrRemoveUserTokenRecoverPassword;
using CidadeInteligente.Mvc.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers;

public class AuthController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("login")]
    public ActionResult Login() => !(User.Identity?.IsAuthenticated ?? false) ? View() : Redirect("/");

    [HttpGet("recover-password")]
    public ActionResult RecoverPassword() => !(User.Identity?.IsAuthenticated ?? false) ? View() : Redirect("/");

    [HttpGet("change-password")]
    public async Task<ActionResult> ChangePassword([FromQuery] string token)
    {
        if (User.Identity?.IsAuthenticated ?? false) return Redirect("/");

        GetOrRemoveUserTokenRecoverPasswordCommand getOrRemoveUserTokenRecoverPasswordCommand = new(token);
        GetOrRemoveUserTokenRecoverPasswordCommandResult? getOrRemoveUserTokenRecoverPasswordCommandResult = await _mediator.Send(getOrRemoveUserTokenRecoverPasswordCommand);
        return View(getOrRemoveUserTokenRecoverPasswordCommandResult);
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<RedirectResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [HttpGet("forbidden")]
    [Authorize]
    public ViewResult Forbidden() => View("~/Views/Error.cshtml", new ErrorViewModel(403, "Você não tem permissão para acessar esta página!"));
}
