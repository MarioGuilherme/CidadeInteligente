using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Application.Queries.GetProjectById;
using CidadeInteligente.Application.Queries.GetProjects;
using CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Mvc.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers;

public class ProjectsController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ViewResult> Index([FromQuery] int page = 1)
    {
        GetProjectsQuery getAllProjectsQuery = new(page);
        GetProjectsQueryResult getProjectsQueryResult = await _mediator.Send(getAllProjectsQuery);
        return View(getProjectsQueryResult);
    }

    [HttpGet("projects/create")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Create()
    {
        GetUsersQueryResult getUsersQueryResult = await _mediator.Send(new GetUsersQuery());
        GetAreasQueryResult getAreasQueryResult = await _mediator.Send(new GetAreasQuery());
        GetCoursesQueryResult getCoursesQueryResult = await _mediator.Send(new GetCoursesQuery());

        ViewBag.Users = getUsersQueryResult.Users;
        ViewBag.Areas = getAreasQueryResult.Areas;
        ViewBag.Courses = getCoursesQueryResult.Courses;

        return View("Form");
    }

    [HttpGet("projects/{projectId:int}/view", Name = "ViewProject")]
    public async Task<ViewResult> View(int projectId)
    {
        GetProjectByIdQuery getProjectDetailsByIdQuery = new(projectId);
        GetProjectByIdQueryResult? getProjectDetailsByIdQueryResult = await _mediator.Send(getProjectDetailsByIdQuery);
        return View(getProjectDetailsByIdQueryResult);
    }

    [HttpGet("projects/{projectId:int}/edit")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Edit(int projectId)
    {
        GetProjectByIdQueryResult? getProjectDetailsByIdQueryResult = await _mediator.Send(new GetProjectByIdQuery(projectId, User.UserId));
        GetUsersQueryResult getUsersQueryResult = await _mediator.Send(new GetUsersQuery());
        GetAreasQueryResult getAreasQueryResult = await _mediator.Send(new GetAreasQuery());
        GetCoursesQueryResult getCoursesQueryResult = await _mediator.Send(new GetCoursesQuery());

        ViewBag.Users = getUsersQueryResult.Users;
        ViewBag.Areas = getAreasQueryResult.Areas;
        ViewBag.Courses = getCoursesQueryResult.Courses;

        return View("Form", getProjectDetailsByIdQueryResult);
    }

    [HttpGet("projects/my")]
    [Authorize]
    public async Task<ViewResult> MyProjects([FromQuery] int page = 1)
    {
        GetRelatedProjectsFromUserQuery getInvolvedProjectsFromUserQuery = new(User.UserId!.Value, page);
        GetRelatedProjectsFromUserQueryResult? getRelatedProjectsFromUserQueryResult = await _mediator.Send(getInvolvedProjectsFromUserQuery);
        return View(getRelatedProjectsFromUserQueryResult);
    }
}
