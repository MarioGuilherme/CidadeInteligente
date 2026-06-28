using CidadeInteligente.Application.Commands.ChangePasswordCommand;
using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Commands.SendEmailRecover;
using CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Mvc.Requests;
using CidadeInteligente.Mvc.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace CidadeInteligente.Mvc.Controllers;

public class AuthController(INotification notification, IMediator mediator) : Controller
{
    private readonly INotification _notification = notification;
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
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [HttpGet("sem-permissao")]
    [Authorize]
    public ViewResult SemPermissao() => View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não tem permissão para acessar está página!"));

    [HttpPost("api/auth/v1/login")]
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

    [HttpPatch("api/auth/v1/sendEmailRecover")]
    public async Task<IActionResult> SendEmailRecover([FromBody] SendEmailRecoverCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("api/auth/v1/changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}