using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Application.Queries.GetProjectById;
using CidadeInteligente.Application.Queries.GetProjects;
using CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CidadeInteligente.Mvc.Controllers;

public class ProjectsController(IMediator mediator) : Controller
{
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
            Log.Error(ex.Message, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("projects/create")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Create()
    {
        try
        {
            GetUsersQueryResult getUsersQueryResult = await _mediator.Send(new GetUsersQuery());
            GetAreasQueryResult getAreasQueryResult = await _mediator.Send(new GetAreasQuery());
            GetCoursesQueryResult getCoursesQueryResult = await _mediator.Send(new GetCoursesQuery());

            ViewBag.Users = getUsersQueryResult.Users;
            ViewBag.Areas = getAreasQueryResult.Areas;
            ViewBag.Courses = getCoursesQueryResult.Courses;

            return View();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("projects/{projectId}/view")]
    public async Task<ViewResult> View(long projectId)
    {
        try
        {
            GetProjectByIdQuery getProjectDetailsByIdQuery = new(projectId, User.UserId);
            GetProjectByIdQueryResult? getProjectDetailsByIdQueryResult = await _mediator.Send(getProjectDetailsByIdQuery);
            return View(getProjectDetailsByIdQueryResult);
        }
        //catch (ProjectNotExistException)
        //{
        //    return View("~/Views/Error.cshtml", new ErrorViewModel(404, "Projeto não encontrado"));
        //}
        catch (Exception ex)
        {
            Log.Error(ex.Message, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("projects/{projectId}/edit")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Edit(long projectId)
    {
        try
        {
            GetProjectByIdQuery getProjectDetailsByIdQuery = new(projectId, User.UserId);
            GetProjectByIdQueryResult? getProjectDetailsByIdQueryResult = await _mediator.Send(getProjectDetailsByIdQuery);
            GetUsersQueryResult getUsersQueryResult = await _mediator.Send(new GetUsersQuery());
            GetAreasQueryResult getAreasQueryResult = await _mediator.Send(new GetAreasQuery());
            GetCoursesQueryResult getCoursesQueryResult = await _mediator.Send(new GetCoursesQuery());

            ViewBag.Users = getUsersQueryResult.Users;
            ViewBag.Areas = getAreasQueryResult.Areas;
            ViewBag.Courses = getCoursesQueryResult.Courses;

            return View(getProjectDetailsByIdQueryResult);
        }
        //catch (ProjectNotExistException)
        //{
        //    return View("~/Views/Error.cshtml", new ErrorViewModel(404, "Projeto não encontrado"));
        //}
        //catch (UserIsReadOnlyException)
        //{
        //    return View("~/Views/Error.cshtml", new ErrorViewModel(403, "Acesso restrito", "Você não está relacionado à este projeto para poder alterá-lo!"));
        //}
        catch (Exception ex)
        {
            Log.Error(ex.Message, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }

    [HttpGet("projects/my")]
    [Authorize]
    public async Task<ViewResult> MyProjects(int page = 1)
    {
        try
        {
            GetRelatedProjectsFromUserQuery getInvolvedProjectsFromUserQuery = new(User.UserId!.Value, page);
            GetRelatedProjectsFromUserQueryResult? getRelatedProjectsFromUserQueryResult = await _mediator.Send(getInvolvedProjectsFromUserQuery);
            return View(getRelatedProjectsFromUserQueryResult);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}