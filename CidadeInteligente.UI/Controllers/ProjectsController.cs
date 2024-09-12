using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourses;
using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Application.Queries.GetDetailsProjectById;
using CidadeInteligente.Application.Queries.GetInvolvedAndCreatedProjectsFromUser;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Models;
using CidadeInteligente.UI.Extensions;
using CidadeInteligente.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

public class ProjectsController(ILogger<ProjectsController> logger, IMediator mediator) : Controller {
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ViewResult> Index(GetAllProjectsQuery getAllProjectsQuery) {
        try {
            PaginationResult<ProjectViewModel> paginationResult = await this._mediator.Send(getAllProjectsQuery);
            return this.View(paginationResult);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("criar-projeto")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Form() {
        try {
            GetAllUsersQuery getAllUsersQuery = new();
            GetAllAreasQuery getAllAreasQuery = new();
            GetAllCoursesQuery getAllCoursesQuery = new();

            this.ViewBag.Users = await this._mediator.Send(getAllUsersQuery);
            this.ViewBag.Areas = await this._mediator.Send(getAllAreasQuery);
            this.ViewBag.Courses = await this._mediator.Send(getAllCoursesQuery);

            return this.View();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("ver-projeto/{projectId}")]
    public async Task<ViewResult> View(long projectId) {
        try {
            GetProjectDetailsByIdQuery getProjectDetailsByIdQuery = new(projectId);
            ProjectDetailsViewModel project = await this._mediator.Send(getProjectDetailsByIdQuery);
            return this.View(project);
        } catch (ProjectNotExistException) {
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(404, "Projeto não encontrado"));
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("editar-projeto/{projectId}")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Edit(long projectId) {
        try {
            GetProjectDetailsByIdQuery getProjectDetailsByIdQuery = new(projectId, this.User.UserId());
            ProjectDetailsViewModel project = await this._mediator.Send(getProjectDetailsByIdQuery);

            GetAllUsersQuery getAllUsersQuery = new();
            GetAllAreasQuery getAllAreasQuery = new();
            GetAllCoursesQuery getAllCoursesQuery = new();

            this.ViewBag.Users = await this._mediator.Send(getAllUsersQuery);
            this.ViewBag.Areas = await this._mediator.Send(getAllAreasQuery);
            this.ViewBag.Courses = await this._mediator.Send(getAllCoursesQuery);

            return this.View(project);
        } catch (ProjectNotExistException) {
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(404, "Projeto não encontrado"));
        } catch (UserIsReadOnlyException) {
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não está relacionado à este projeto para poder alterá-lo!"));
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("meus-projetos")]
    [Authorize]
    public async Task<ViewResult> MyProjects(int page = 1) {
        try {
            GetInvolvedAndCreatedProjectsFromUserQuery getInvolvedProjectsFromUserQuery = new(this.User.UserId(), page);
            PaginationResult<ProjectViewModel> paginationResult = await this._mediator.Send(getInvolvedProjectsFromUserQuery);
            return this.View(paginationResult);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}