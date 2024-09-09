using CidadeInteligente.Application.Queries.GetInvolvedProjectsFromUser;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Models;
using CidadeInteligente.UI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

public class UsersController(ILogger<UsersController> logger, IMediator mediator) : Controller {
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login() => this.View();

    [HttpGet("meus-projetos")]
    public async Task<ViewResult> MyProjects(int page = 1) {
        GetInvolvedProjectsFromUserQuery getInvolvedProjectsFromUserQuery = new(this.User.UserId(), page);
        PaginationResult<ProjectViewModel> paginationResult = await this._mediator.Send(getInvolvedProjectsFromUserQuery);
        return this.View(paginationResult);
    }
}