using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Queries.GetCreatedProjectsFromUser;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
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
    public async Task<ViewResult> MyProjects() {
        GetCreatedProjectsFromUserQuery getCreatedProjectsFromUserQuery = new(this.User.UserId());
        List<Project> projects = await this._mediator.Send(getCreatedProjectsFromUserQuery);

        PaginatedView<Project> paginatedView = new();
        paginatedView.Data = projects;
        paginatedView.CurrentPage = 1;
        paginatedView.TotalPages = 22;

        return this.View(paginatedView);
    }
}