using CidadeInteligente.UI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

public class AuthController : Controller {
    [HttpGet("login")]
    public IActionResult Login() => !(this.User.Identity?.IsAuthenticated ?? false) ? this.View() : this.Redirect("/");

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