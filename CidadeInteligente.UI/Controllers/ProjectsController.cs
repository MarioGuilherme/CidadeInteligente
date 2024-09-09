using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourses;
using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Application.Queries.GetDetailsProjectById;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CidadeInteligente.UI.Controllers;

public class ProjectsController(ILogger<ProjectsController> logger, IMediator mediator) : Controller {
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ViewResult> Index(GetAllProjectsQuery getAllProjectsQuery) {
        PaginationResult<ProjectViewModel> paginationResult = await this._mediator.Send(getAllProjectsQuery);
        return this.View(paginationResult);
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
        GetProjectDetailsByIdQuery getProjectDetailsByIdQuery = new(projectId);
        ProjectDetailsViewModel? project = await this._mediator.Send(getProjectDetailsByIdQuery);

        if (project is null) return this.View("/Shared/Error");

        return this.View(project);
    }

    [HttpGet("editar-projeto/{projectId}")]
    public async Task<ViewResult> Edit(long projectId) {
        GetProjectDetailsByIdQuery getProjectDetailsByIdQuery = new(projectId);
        ProjectDetailsViewModel? project = await this._mediator.Send(getProjectDetailsByIdQuery);

        if (project is null)
            return this.View("/Shared/Error");

        GetAllUsersQuery getAllUsersQuery = new();
        GetAllAreasQuery getAllAreasQuery = new();
        GetAllCoursesQuery getAllCoursesQuery = new();

        this.ViewBag.Users = await this._mediator.Send(getAllUsersQuery);
        this.ViewBag.Areas = await this._mediator.Send(getAllAreasQuery);
        this.ViewBag.Courses = await this._mediator.Send(getAllCoursesQuery);

        return this.View(project);
    }
}