using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Application.Commands.DeleteProjectById;
using CidadeInteligente.Application.Commands.UpdateProject;
using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Application.Queries.GetProjectById;
using CidadeInteligente.Application.Queries.GetProjects;
using CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.Requests;
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

    [HttpGet("criar-projeto")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Form()
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

    [HttpGet("ver-projeto/{projectId}")]
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

    [HttpGet("editar-projeto/{projectId}")]
    [Authorize(Roles = nameof(Role.Teacher))]
    public async Task<ViewResult> Edit(long projectId)
    {
        try
        {
            GetProjectByIdQuery getProjectDetailsByIdQuery = new(projectId, User.UserId);
            GetProjectByIdQueryResult? getProjectDetailsByIdQueryResult = await _mediator.Send(getProjectDetailsByIdQuery);

            GetUsersQuery getAllUsersQuery = new();
            GetAreasQuery getAllAreasQuery = new();
            GetCoursesQuery getAllCoursesQuery = new();

            ViewBag.Users = await _mediator.Send(getAllUsersQuery);
            ViewBag.Areas = await _mediator.Send(getAllAreasQuery);
            ViewBag.Courses = await _mediator.Send(getAllCoursesQuery);

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

    [HttpGet("meus-projetos")]
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

    [HttpPost("api/projects")]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        CreateProjectCommand createProjectCommand = new(request.Title,
            request.AreaId,
            request.CourseId,
            (long)User.UserId!,
            request.Description,
            request.StartedAt,
            request.FinishedAt,
            request.InvolvedUsers,
            request.Medias.Select(m => new CreateProjectCommand.CreateMediaCommand(m.Title,
                m.Description,
                m.Extension,
                m.Base64)));
        long projectId = await _mediator.Send(createProjectCommand);
        Response.Headers.Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/ver-projeto/{projectId}";
        return StatusCode(201);
    }

    [HttpPatch("api/projects/{projectId:int}")]
    public async Task<ActionResult> UpdateProject(long projectId, [FromBody] UpdateProjectRequest request)
    {
        UpdateProjectCommand updateProjectCommand = new(projectId,
            User.UserId,
            request.Title,
            request.AreaId,
            request.CourseId,
            request.Description,
            request.StartedAt,
            request.FinishedAt,
            request.InvolvedUsers,
            request.Medias.Select(media => new UpdateProjectCommand.UpdateMediaCommand(media.MediaId,
                media.Title,
                media.Description,
                media.Extension,
                media.Path,
                media.Base64)));
        await _mediator.Send(updateProjectCommand);
        return NoContent();
    }

    [HttpDelete("api/projects/{projectId:int}")]
    public async Task<ActionResult> DeleteProject(long projectId)
    {
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId, User.UserId);
        await _mediator.Send(deleteProjectByIdCommand);
        return NoContent();
    }
}