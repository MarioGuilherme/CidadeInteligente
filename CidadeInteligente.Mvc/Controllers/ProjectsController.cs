using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Application.Queries.GetProjectById;
using CidadeInteligente.Application.Queries.GetProjects;
using CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers;

public class ProjectsController(ILogger<ProjectsController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ViewResult> Index(GetProjectsQuery getAllProjectsQuery)
    {
        try
        {
            GetProjectsQueryResult getProjectsQueryResult = await _mediator.Send(getAllProjectsQuery);
            return View(getProjectsQueryResult);
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
            ViewBag.Users = await _mediator.Send(new GetUsersQuery());
            ViewBag.Areas = await _mediator.Send(new GetAreasQuery());
            ViewBag.Courses = await _mediator.Send(new GetCoursesQuery());

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
            GetProjectByIdQuery getProjectDetailsByIdQuery = new(projectId);
            GetProjectByIdQueryResult getProjectDetailsByIdQueryResult = await _mediator.Send(getProjectDetailsByIdQuery);
            return View(getProjectDetailsByIdQueryResult);
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
            GetProjectByIdQuery getProjectDetailsByIdQuery = new(projectId, User.UserId());
            GetProjectByIdQueryResult getProjectDetailsByIdQueryResult = await _mediator.Send(getProjectDetailsByIdQuery);

            GetUsersQuery getAllUsersQuery = new();
            GetAreasQuery getAllAreasQuery = new();
            GetCoursesQuery getAllCoursesQuery = new();

            ViewBag.Users = await _mediator.Send(getAllUsersQuery);
            ViewBag.Areas = await _mediator.Send(getAllAreasQuery);
            ViewBag.Courses = await _mediator.Send(getAllCoursesQuery);

            return View(getProjectDetailsByIdQueryResult);
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
            GetRelatedProjectsFromUserQuery getInvolvedProjectsFromUserQuery = new(User.UserId(), page);
            GetRelatedProjectsFromUserQueryResult getRelatedProjectsFromUserQueryResult = await _mediator.Send(getInvolvedProjectsFromUserQuery);
            return View(getRelatedProjectsFromUserQueryResult);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}