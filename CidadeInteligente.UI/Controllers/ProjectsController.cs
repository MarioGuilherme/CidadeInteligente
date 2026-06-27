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

public class ProjectsController(ILogger<ProjectsController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ViewResult> Index(GetAllProjectsQuery getAllProjectsQuery)
    {
        try
        {
            PaginationResult<ProjectViewModel> paginationResult = await _mediator.Send(getAllProjectsQuery);
            return View(paginationResult);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("criar-projeto")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Form()
    {
        try
        {
            GetAllUsersQuery getAllUsersQuery = new();
            GetAllAreasQuery getAllAreasQuery = new();
            GetAllCoursesQuery getAllCoursesQuery = new();

            ViewBag.Users = await _mediator.Send(getAllUsersQuery);
            ViewBag.Areas = await _mediator.Send(getAllAreasQuery);
            ViewBag.Courses = await _mediator.Send(getAllCoursesQuery);

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("ver-projeto/{projectId}")]
    public async Task<ViewResult> View(long projectId)
    {
        try
        {
            GetProjectDetailsByIdQuery getProjectDetailsByIdQuery = new(projectId);
            ProjectDetailsViewModel project = await _mediator.Send(getProjectDetailsByIdQuery);
            return View(project);
        }
        catch (ProjectNotExistException)
        {
            return View("~/Views/Error.cshtml", new ErrorViewModel(404, "Projeto não encontrado"));
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("editar-projeto/{projectId}")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Edit(long projectId)
    {
        try
        {
            GetProjectDetailsByIdQuery getProjectDetailsByIdQuery = new(projectId, User.UserId());
            ProjectDetailsViewModel project = await _mediator.Send(getProjectDetailsByIdQuery);

            GetAllUsersQuery getAllUsersQuery = new();
            GetAllAreasQuery getAllAreasQuery = new();
            GetAllCoursesQuery getAllCoursesQuery = new();

            ViewBag.Users = await _mediator.Send(getAllUsersQuery);
            ViewBag.Areas = await _mediator.Send(getAllAreasQuery);
            ViewBag.Courses = await _mediator.Send(getAllCoursesQuery);

            return View(project);
        }
        catch (ProjectNotExistException)
        {
            return View("~/Views/Error.cshtml", new ErrorViewModel(404, "Projeto não encontrado"));
        }
        catch (UserIsReadOnlyException)
        {
            return View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não está relacionado à este projeto para poder alterá-lo!"));
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("meus-projetos")]
    [Authorize]
    public async Task<ViewResult> MyProjects(int page = 1)
    {
        try
        {
            GetInvolvedAndCreatedProjectsFromUserQuery getInvolvedProjectsFromUserQuery = new(User.UserId(), page);
            PaginationResult<ProjectViewModel> paginationResult = await _mediator.Send(getInvolvedProjectsFromUserQuery);
            return View(paginationResult);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}