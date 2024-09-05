using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourses;
using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Application.Queries.GetProjectById;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

public class ProjectsController(ILogger<ProjectsController> logger, IMediator mediator) : Controller {
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ViewResult> Index() {
        GetAllProjectsQuery getAllProjectsQuery = new();

        this.ViewBag.Projects = await this._mediator.Send(getAllProjectsQuery);
        PaginatedView<ProjectViewModel> paginatedView = new PaginatedView<ProjectViewModel>();
        paginatedView.Data = await this._mediator.Send(getAllProjectsQuery);
        paginatedView.CurrentPage = 1;
        paginatedView.TotalPages = 22;

        return this.View(paginatedView);
    }

    [HttpGet("criar-projeto")]
    [Authorize(Roles = "Teacher")]
    public async Task<ViewResult> Form() {
        GetAllUsersQuery getAllUsersQuery = new();
        GetAllAreasQuery getAllAreasQuery = new();
        GetAllCoursesQuery getAllCoursesQuery = new();

        this.ViewBag.Users = await this._mediator.Send(getAllUsersQuery);
        this.ViewBag.Areas = await this._mediator.Send(getAllAreasQuery);
        this.ViewBag.Courses = await this._mediator.Send(getAllCoursesQuery);

        return this.View();
    }

    [HttpGet("ver-projeto/{projectId}")]
    public async Task<ViewResult> View(long projectId) {
        GetProjectByIdQuery getProjectByIdQuery = new(projectId);
        Project? project = await this._mediator.Send(getProjectByIdQuery);

        if (project is null) return this.View("/Shared/Error");

        return this.View(project);
    }

    [HttpGet("editar-projeto/{projectId}")]
    public async Task<ViewResult> Edit(long projectId) {
        GetProjectByIdQuery getProjectByIdQuery = new(projectId);

        Project? project = await this._mediator.Send(getProjectByIdQuery);

        if (project is null)
            return this.View("/Shared/Error");

        GetAllUsersQuery getAllUsersQuery = new();
        GetAllAreasQuery getAllAreasQuery = new();
        GetAllCoursesQuery getAllCoursesQuery = new();
        this.ViewBag.OthersUsers = (await this._mediator.Send(getAllUsersQuery)).Except(project.InvolvedUsers).ToList();
        this.ViewBag.Areas = await this._mediator.Send(getAllAreasQuery);
        this.ViewBag.Courses = await this._mediator.Send(getAllCoursesQuery);

        return this.View(project);
    }
}